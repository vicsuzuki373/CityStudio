using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEngine.Video;

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
    public GameObject jayWalkerPrefab;
    public GameObject flipPrompt;
    bool jayDirection;

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


    [Header("Controller")]
    [Range(0.01f, 0.5f)]
    public float autoScrollSpeed;
    float vertical, horizontal;
    bool aButton, bButton, yButton, rightStickButton, dup, ddown;
    List<Scrollbar> dropDownList;

    public static bool startEditor = false;

    [Header("ToolTips")]
    public GameObject TTwindow;
    TextMeshProUGUI TTtext;
    public List<VideoClip> TTclips;
    VideoPlayer TTplayer;
    public Toggle TTtoggle;

    void Start()
    {
        lightPieces = new List<Light>();
        streetLights.GetComponentsInChildren(lightPieces);
        lightSwitch = false;
        worldVolume.profile.TryGet(out worldBloom);

        camSwitch = true;
        forward = true;
        uiRaycast = Canvas.GetComponent<GraphicRaycaster>();
        jayDirection = true;

        vertical = 0;
        horizontal = 0;
        aButton = false;
        bButton = false;
        yButton = false;
        rightStickButton = false;
        dup = false;
        ddown = false;

        dropDownList = new List<Scrollbar>();

        TTtext = TTwindow.GetComponentInChildren<TextMeshProUGUI>();
        TTplayer = TTwindow.GetComponentInChildren<VideoPlayer>();
    }

    void Update()
    {
        cameraSwitch();
        radioChannel();
        TTwindow.SetActive(TTtoggle.isOn);
    }

    private void FixedUpdate()
    {
        cameraControls();
    }

    void getControllerInput()
    {
        if (Gamepad.current == null)
        {//reset the controller
            vertical = 0;
            horizontal = 0;
            aButton = false;
            bButton = false;
            yButton = false;
            rightStickButton = false;
            dup = false;
            ddown = false;
            return;
        }
        
        vertical = Gamepad.current.rightStick.ReadValue().y;
        horizontal = Gamepad.current.rightStick.ReadValue().x;
        aButton = Gamepad.current.aButton.isPressed;
        bButton = Gamepad.current.bButton.isPressed;
        yButton = Gamepad.current.yButton.isPressed;
        rightStickButton = Gamepad.current.rightStickButton.isPressed;
        dup = Gamepad.current.dpad.up.isPressed;
        ddown = Gamepad.current.dpad.down.isPressed;

        dropDownPrompt.GetComponentsInChildren(dropDownList);
        autoScrollDropDown();
    }

    void autoScrollDropDown()
    {
        foreach(Scrollbar bar in dropDownList)
        {
            bar.value -= (autoScrollSpeed * bar.size) * Time.deltaTime;
            if (bar.value <= 0.0f)
            {
                StartCoroutine("redoAutoScroll", 2.0f);
            }
        }
    }

    void cameraSwitch()
    {
        if (startEditor) // maybe change trigger later
        {
            startEditor = false;
            overheadCam.SetActive(camSwitch);
            Canvas.SetActive(camSwitch);
            playerCam.SetActive(!camSwitch);
            camSwitch = !camSwitch;
        }
    }

    void cameraControls()
    {
        if (!overheadCam.activeInHierarchy) // overheadCam gatekeeper, everything below req. overheadCam active
        {  return; }

        selectorRC();
        getControllerInput();

        if (overheadCam.transform.position.z <= -385.0f)
        {
            if (forward == true)
                overheadCam.transform.eulerAngles = new Vector3(overheadCam.transform.eulerAngles.x, Mathf.LerpAngle(overheadCam.transform.eulerAngles.y, 150.0f, Time.deltaTime * 15.0f), overheadCam.transform.eulerAngles.z);
            else
            {
                overheadCam.transform.eulerAngles = new Vector3(overheadCam.transform.eulerAngles.x, Mathf.LerpAngle(overheadCam.transform.eulerAngles.y, -30.0f, Time.deltaTime * 15.0f), overheadCam.transform.eulerAngles.z);
            }
        }
        else
        {
            if (forward == true)
                overheadCam.transform.eulerAngles = new Vector3(overheadCam.transform.eulerAngles.x, Mathf.LerpAngle(overheadCam.transform.eulerAngles.y, 180.0f, Time.deltaTime * 15.0f), overheadCam.transform.eulerAngles.z);
            else
                overheadCam.transform.eulerAngles = new Vector3(overheadCam.transform.eulerAngles.x, Mathf.LerpAngle(0.0f, overheadCam.transform.eulerAngles.y, Time.deltaTime * 15.0f), overheadCam.transform.eulerAngles.z);
        }

        if (Input.GetKey(KeyCode.LeftShift) || rightStickButton)
        {
            camSpeedMulti = camSpeedMaxMulti;
        }
        else
        { camSpeedMulti = 1.0f; }

        if (Input.GetKey(KeyCode.W) || vertical >= 0.1f)
        {
            overheadCam.transform.Translate(Vector3.forward * Time.deltaTime * camSpeed * camSpeedMulti, Space.Self);
        }
        else if (Input.GetKey(KeyCode.S) || vertical <= -0.1f)
        {
            overheadCam.transform.Translate(-Vector3.forward * Time.deltaTime * camSpeed * camSpeedMulti, Space.Self);
        }

        if (Input.GetKey(KeyCode.A) || horizontal <= -0.1f)
        {
            overheadCam.transform.Translate(-Vector3.right * Time.deltaTime * camSpeed * camSpeedMulti, Space.Self);
        }
        else if (Input.GetKey(KeyCode.D) || horizontal >= 0.1f)
        {
            overheadCam.transform.Translate(Vector3.right * Time.deltaTime * camSpeed * camSpeedMulti, Space.Self);
        }

        if (Input.GetKeyDown(KeyCode.Q) || yButton)
        {
            forward = !forward;
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 || dup)
        {
            overheadCam.transform.Translate(Vector3.up * Time.deltaTime * camSpeed, Space.World);
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 || ddown)
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
 

        if ((Input.GetMouseButton(0) || aButton) && uiResults.Count == 0) //leftclick
        {
            ray = overheadCam.GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition); // raycast from screen position of mouse
            Physics.Raycast(ray, out hit);

            if (hit.collider != null && hit.collider.tag == "igEditor") // make object selected if tagged igEditor
            {
                selected = hit.collider.gameObject;
                entityTypeSelection.text = selected.name;

                switchSelection();
            }
            else if (selected != null && selected.GetComponent<igEditorUI>().entityType != 3) // 3 is radio, cant move that
            {
                switch(selected.GetComponent<igEditorUI>().entityType)
                {
                    case 1:
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
                        break;
                    case 2:
                        selected.transform.position = hit.point;
                        break;
                    case 4:
                        if (selected.transform.position.z <= -385.0f)
                        {
                            selected.transform.position = new Vector3(hit.point.x, hit.point.y + selected.GetComponent<Collider>().bounds.extents.y, hit.point.z);
                            selected.GetComponent<pedestrianObject>().initialJayWalkerPosi = selected.transform.position;
                            if(jayDirection)
                                selected.transform.rotation = Quaternion.Euler(0, -120.0f, -0);
                            else
                                selected.transform.rotation = Quaternion.Euler(0, 60.0f, -0);
                        }
                        else
                        {
                            selected.transform.position = new Vector3(hit.point.x, hit.point.y + selected.GetComponent<Collider>().bounds.extents.y, hit.point.z);
                            selected.GetComponent<pedestrianObject>().initialJayWalkerPosi = selected.transform.position;
                            if (jayDirection)
                                selected.transform.rotation = Quaternion.Euler(0, -90.0f, 0.0f);
                            else
                                selected.transform.rotation = Quaternion.Euler(0, 90.0f, 0.0f);
                        }
                        break;
                }
                
            }
        }
        else if ((Input.GetMouseButton(1) || bButton) && uiResults.Count == 0) //rightclick
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
                flipPrompt.SetActive(false);
                deletePrompt.SetActive(true);
                radioChannelPrompt.gameObject.SetActive(false);
                break;
            case 2:
                //VideoPlayerPrompt.SetActive(false);
                SpeedNumberPrompt.SetActive(true);
                dropDownPrompt.SetActive(false);
                flipPrompt.SetActive(false);
                deletePrompt.SetActive(false);
                radioChannelPrompt.gameObject.SetActive(false);
                break;
            case 3:
                SpeedNumberPrompt.SetActive(false);
                dropDownPrompt.SetActive(true);
                flipPrompt.SetActive(false);
                deletePrompt.SetActive(false);
                radioChannelPrompt.gameObject.SetActive(true);
                break;
            case 4:
                SpeedNumberPrompt.SetActive(false);
                dropDownPrompt.SetActive(false);
                flipPrompt.SetActive(true);
                deletePrompt.SetActive(true);
                radioChannelPrompt.gameObject.SetActive(false);
                break;
            default:
                //VideoPlayerPrompt.SetActive(false); // set everything inactive if there is an err in entityType
                SpeedNumberPrompt.SetActive(false);
                dropDownPrompt.SetActive(false);
                flipPrompt.SetActive(false);
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

    public void toggleNewJayWalker()
    {
        selected = Instantiate(jayWalkerPrefab);
        selected.transform.parent = GameObject.Find("pedestrianPool").transform;
        selected.SetActive(true);
        selected.name = jayWalkerPrefab.name;
        entityTypeSelection.text = selected.name;
        switchSelection();
    }

    public void changeJayDirection()
    {
        jayDirection = !jayDirection;
    }

    public void delete()
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

    public void changeSeed(string val)
    {
        List<char> characters = new List<char>();
        characters.AddRange(val);

        int output = 0;

        foreach(char chars in characters)
        {
            output += (int)chars;
        }

        Random.InitState(output);
        Debug.Log("Random seed set to: " + output);
    }

    public void TTbuttons(int index)
    {
        switch(index)
        {
            case 0:
                TTplayer.clip = TTclips[index];
                TTtext.text = "Use the keyboard keys; W, A, S, D to move editor camera around the scene. Holding Shift key will allow for faster movement.";
                break;
            case 1:
                TTplayer.clip = TTclips[index];
                TTtext.text = "Use the mouse scrollwheel to move editor camera higher and lower in the scene. The camera can continue to ascend / descend until a maximum offset is reached.";
                break;
            case 2:
                TTplayer.clip = TTclips[index];
                TTtext.text = "Use the mouse left click to select editable objects in the scene. Once an object is selected continue using mouse left click to move the object or select anew.";
                break;
            case 3:
                TTplayer.clip = TTclips[index];
                TTtext.text = "Use the mouse right click to deselect editable objects in the scene. Once an object is deselected they will no longer be edited or moved until reselected.";
                break;
            default:
                TTplayer.clip = TTclips[index];
                TTtext.text = "Use the keyboard keys; W, A, S, D to move editor camera around the scene. Holding Shift key will allow for faster movement.";
                break;
        }
    }

    private IEnumerator redoAutoScroll(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        foreach (Scrollbar bar in dropDownList)
        {
            bar.value = 1.0f;
        }
    }
}
