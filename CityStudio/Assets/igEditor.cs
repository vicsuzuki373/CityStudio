using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class igEditor : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject VideoPlayerPrompt;
    GraphicRaycaster uiRaycast;
    PointerEventData uiPointerEventData;
    public EventSystem uiEventSystem;
    public TextMeshProUGUI entityTypeSelection;

    public GameObject overheadCam;
    public GameObject playerCam;
    bool camSwitch;
    public float camSpeed;
    public float maxCamHeight;
    public float minCamHeight;

    Ray ray;
    RaycastHit hit;
    public GameObject selected;

    void Start()
    {
        camSwitch = true;
        uiRaycast = Canvas.GetComponent<GraphicRaycaster>();
    }

    private void FixedUpdate()
    {
        cameraControls();
    }

    void cameraSwitch()
    {
        if (Input.GetKeyUp(KeyCode.P)) // maybe change trigger later
        {
            camSwitch = !camSwitch;
            overheadCam.SetActive(camSwitch);
            Canvas.SetActive(camSwitch);
            playerCam.SetActive(!camSwitch);
        }
    }

    void cameraControls()
    {
        cameraSwitch();

        if (!overheadCam.activeInHierarchy) // overheadCam gatekeeper, everything below req. overheadCam active
            return;

        selectorRC();

        if (Input.GetKey(KeyCode.W))
        {
            overheadCam.transform.Translate(-Vector3.forward * Time.deltaTime * camSpeed, Space.World);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            overheadCam.transform.Translate(Vector3.forward * Time.deltaTime * camSpeed, Space.World);
        }

        if (Input.GetKey(KeyCode.A))
        {
            overheadCam.transform.Translate(Vector3.right * Time.deltaTime * camSpeed, Space.World);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            overheadCam.transform.Translate(-Vector3.right * Time.deltaTime * camSpeed, Space.World);
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
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
            ray = overheadCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition); // raycast from screen position of mouse
            Physics.Raycast(ray, out hit);

            if (hit.collider != null && hit.collider.tag == "igEditor") // make object selected if tagged igEditor
            {
                selected = hit.collider.gameObject;
                entityTypeSelection.text = selected.name;

                switch (selected.GetComponent<igEditorUI>().entityType)
                {
                    case 1:
                        VideoPlayerPrompt.SetActive(true); // set active for this case and deactivate all irrelevant UI fields
                        break;
                    default:
                        VideoPlayerPrompt.SetActive(false); // set everything inactive if there is an err in entityType
                        break;
                }
            }
            else if (selected != null)
                selected.transform.position = hit.point;
        }
        else if (Input.GetMouseButton(1) && uiResults.Count == 0) //rightclick
        {
            selected = null; // unselect
            entityTypeSelection.text = "Nothing Selected";
            VideoPlayerPrompt.SetActive(false); //set everything inactive because nothing selected
        }
    }

    public void editInfo(string _info)
    {
        try
        {
            selected.GetComponent<igEditorUI>().changeInfo(_info);
        }
        catch { }
    }
}
