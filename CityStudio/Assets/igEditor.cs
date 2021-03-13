using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class igEditor : MonoBehaviour
{
    [Header("Raycast Systems")]
    public EventSystem uiEventSystem;
    public TextMeshProUGUI entityTypeSelection;
    GraphicRaycaster uiRaycast;
    PointerEventData uiPointerEventData;
    Ray ray;
    RaycastHit hit;
    public GameObject selected;

    [Header("UI Prompts")]
    public GameObject Canvas;
    public GameObject dropDownPrompt;
    public GameObject SpeedNumberPrompt;
    bool lightSwitch;
    public GameObject radioObject;
    public Dropdown radioChannelPrompt;
    public List<string> radioChanOptions;
    public GameObject billboardPrefab;
    public GameObject deletePrompt;


    [Header("Post Processing")]
    public GameObject streetLights;
    List<Light> lightPieces;
    public Volume worldVolume;
    Bloom worldBloom;
    public Light worldLightSun;
    public Light worldLightMoon;
    

    [Header("Camera")]
    public GameObject overheadCam;
    public GameObject playerCam;
    bool camSwitch;
    public float camSpeed;
    public float camSpeedMaxMulti;
    float camSpeedMulti;
    public float maxCamHeight;
    public float minCamHeight;
    bool forward;

    void Start()
    {
        lightPieces = new List<Light>();
        streetLights.GetComponentsInChildren(lightPieces);
        lightSwitch = false;
        worldVolume.profile.TryGet(out worldBloom);

        camSwitch = true;
        forward = true;
        uiRaycast = Canvas.GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        cameraSwitch();
        radioChannel();
    }

    private void FixedUpdate()
    {
        cameraControls();
    }

    void cameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.P)) // maybe change trigger later
        {
            camSwitch = !camSwitch;
            overheadCam.SetActive(camSwitch);
            Canvas.SetActive(camSwitch);
            playerCam.SetActive(!camSwitch);
        }
    }

    void cameraControls()
    {
        if (!overheadCam.activeInHierarchy) // overheadCam gatekeeper, everything below req. overheadCam active
        { Cursor.lockState = CursorLockMode.Locked; return; }
        Cursor.lockState = CursorLockMode.None;

        selectorRC();

        if(overheadCam.transform.position.z <= -385.0f)
        {
            if (forward == true)
                overheadCam.transform.eulerAngles = new Vector3(overheadCam.transform.eulerAngles.x, Mathf.LerpAngle(overheadCam.transform.eulerAngles.y, 150.0f, Time.deltaTime * 15.0f), overheadCam.transform.eulerAngles.z);
            else
            {
                Debug.Log("scream a bit");
                overheadCam.transform.eulerAngles = new Vector3(overheadCam.transform.eulerAngles.x, Mathf.LerpAngle(overheadCam.transform.eulerAngles.y, -30.0f, Time.deltaTime * 15.0f), overheadCam.transform.eulerAngles.z);
            }
        }
        else
        {
            if(forward == true)
                overheadCam.transform.eulerAngles = new Vector3(overheadCam.transform.eulerAngles.x, Mathf.LerpAngle(overheadCam.transform.eulerAngles.y, 180.0f, Time.deltaTime * 15.0f), overheadCam.transform.eulerAngles.z);
            else
                overheadCam.transform.eulerAngles = new Vector3(overheadCam.transform.eulerAngles.x, Mathf.LerpAngle(0.0f, overheadCam.transform.eulerAngles.y, Time.deltaTime * 15.0f), overheadCam.transform.eulerAngles.z);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            camSpeedMulti = camSpeedMaxMulti;
        }
        else
        { camSpeedMulti = 1.0f; }

        if (Input.GetKey(KeyCode.W))
        {
            overheadCam.transform.Translate(Vector3.forward * Time.deltaTime * camSpeed * camSpeedMulti, Space.Self);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            overheadCam.transform.Translate(-Vector3.forward * Time.deltaTime * camSpeed * camSpeedMulti, Space.Self);
        }

        if (Input.GetKey(KeyCode.A))
        {
            overheadCam.transform.Translate(-Vector3.right * Time.deltaTime * camSpeed * camSpeedMulti, Space.Self);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            overheadCam.transform.Translate(Vector3.right * Time.deltaTime * camSpeed * camSpeedMulti, Space.Self);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            forward = !forward;
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            overheadCam.transform.Translate(Vector3.up * Time.deltaTime * camSpeed, Space.World);
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            overheadCam.transform.Translate(Vector3.down * Time.deltaTime * camSpeed, Space.World);
        }
        overheadCam.transform.position = new Vector3(overheadCam.transform.position.x, Mathf.Clamp(overheadCam.transform.position.y, minCamHeight, maxCamHeight), overheadCam.transform.position.z);
    }

    void selectorRC()
    {
        uiPointerEventData = new PointerEventData(uiEventSystem); //uiRaycast so that ui elements WILL stop scene raycast if user is selecting elements in UI
        uiPointerEventData.position = Input.mousePosition;
        List<RaycastResult> uiResults = new List<RaycastResult>();
        uiRaycast.Raycast(uiPointerEventData, uiResults);
 

        if (Input.GetMouseButton(0) && uiResults.Count == 0) //leftclick
        {
            ray = overheadCam.GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition); // raycast from screen position of mouse
            Physics.Raycast(ray, out hit);

            if (hit.collider != null && hit.collider.tag == "igEditor") // make object selected if tagged igEditor
            {
                selected = hit.collider.gameObject;
                entityTypeSelection.text = selected.name;

                switchSelection();
            }
            else if (selected != null && selected.GetComponent<igEditorUI>().entityType != 3)
            {
                if (selected.transform.position.z <= -385.0f)
                {
                    selected.transform.position = hit.point;
                    selected.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, -30.0f);
                }
                else
                {
                    selected.transform.position = hit.point;
                    selected.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
                }
            }
        }
        else if (Input.GetMouseButton(1) && uiResults.Count == 0) //rightclick
        {
            selected = null; // unselect
            entityTypeSelection.text = "Nothing Selected";
            dropDownPrompt.SetActive(false); //set everything inactive because nothing selected
            SpeedNumberPrompt.SetActive(false);
            deletePrompt.SetActive(false);
            radioChannelPrompt.gameObject.SetActive(false);
        }
    }

    void switchSelection()
    {
        switch (selected.GetComponent<igEditorUI>().entityType)
        {
            case 1:
                //VideoPlayerPrompt.SetActive(true); // set active for this case and deactivate all irrelevant UI fields
                SpeedNumberPrompt.SetActive(false);
                dropDownPrompt.SetActive(true);
                deletePrompt.SetActive(true);
                radioChannelPrompt.gameObject.SetActive(false);
                break;
            case 2:
                //VideoPlayerPrompt.SetActive(false);
                SpeedNumberPrompt.SetActive(true);
                dropDownPrompt.SetActive(false);
                deletePrompt.SetActive(false);
                radioChannelPrompt.gameObject.SetActive(false);
                break;
            case 3:
                SpeedNumberPrompt.SetActive(false);
                dropDownPrompt.SetActive(true);
                deletePrompt.SetActive(false);
                radioChannelPrompt.gameObject.SetActive(true);
                break;
            default:
                //VideoPlayerPrompt.SetActive(false); // set everything inactive if there is an err in entityType
                SpeedNumberPrompt.SetActive(false);
                dropDownPrompt.SetActive(false);
                deletePrompt.SetActive(false);
                radioChannelPrompt.gameObject.SetActive(false);
                break;
        }
    }

    public void editInfo(string info)
    {
        try
        {
            if(selected.GetComponent<igEditorUI>().entityType == 1)
                this.GetComponent<fileExplorer>().applySelection(selected, 0);
            else if(selected.GetComponent<igEditorUI>().entityType == 2)
            {
                selected.GetComponent<igEditorUI>().changeInfo(info);
            }
            else if (selected.GetComponent<igEditorUI>().entityType == 3)
            {
                this.GetComponent<fileExplorer>().applySelection(selected, radioChannelPrompt.value);
            }
        }
        catch { Debug.Log("error in editinfo"); }
    }

    public void toggleLight()
    {
        lightSwitch = !lightSwitch;

        if(lightSwitch == true)
        {
            worldBloom.intensity.value = 15.0f;
            worldLightSun.gameObject.SetActive(false);
            worldLightMoon.gameObject.SetActive(true);
            foreach(Light _lights in lightPieces)
            {
                _lights.intensity = 5.0f;
            }
            RenderSettings.ambientIntensity = 0.0f;
            playerCam.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
                
        }
        else
        {
            worldBloom.intensity.value = 0.0f;
            worldLightSun.gameObject.SetActive(true);
            worldLightMoon.gameObject.SetActive(false);
            foreach (Light _lights in lightPieces)
            {
                _lights.intensity = 0.0f;
            }
            RenderSettings.ambientIntensity = 1.0f;
            playerCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        }
    }

    public void toggleRadio()
    {
        selected = radioObject;
        entityTypeSelection.text = selected.name;
        switchSelection();
    }

    public void toggleNewBillboard()
    {
        selected = Instantiate(billboardPrefab);
        selected.SetActive(true);
        selected.name = billboardPrefab.name;
        entityTypeSelection.text = selected.name;
        switchSelection();
    }

    public void deleteBillboard()
    {
        Destroy(selected);
        selected = null;
        entityTypeSelection.text = "Nothing Selected";
        dropDownPrompt.SetActive(false); //set everything inactive because nothing selected
        SpeedNumberPrompt.SetActive(false);
        deletePrompt.SetActive(false);
        radioChannelPrompt.gameObject.SetActive(false);
    }

    public void radioChannel()
    {
        if (radioObject.GetComponent<Radio>().sounds.Count > radioChanOptions.Count - 1)
        {
            radioChanOptions.Clear();
            radioChannelPrompt.ClearOptions();
            for (int i = 0; i < radioObject.GetComponent<Radio>().sounds.Count + 1; i++)
            {
                radioChanOptions.Add("CH " + i);
            }
            radioChannelPrompt.AddOptions(radioChanOptions);
        }
        else
            radioChannelPrompt.GetComponentInChildren<Text>().text = radioChannelPrompt.options[radioChannelPrompt.value].text;
    }
}
