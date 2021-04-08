using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class igEditorController : MonoBehaviour
{
    public float virtualMouseSpd;

    [Range(0.0f, 1.0f)]
    public float joystickDetect;

    Vector2 virtualMousePos;
    bool updatePos;
    bool isPressed;

    public GraphicRaycaster rC; // canvas gameobject
    PointerEventData ptrEventData;
    public EventSystem eventSys; // event system gameobject

    List<RaycastResult> results;

    // Package Manager -> Input System -> Add
    // Edit -> Project Settings -> Other Settings -> Configuration -> Active Input Handling -> Both

    //IMPORTANT: in event system object, DO NOT click "Replace with InputSystemUIInputModule", DOING SO WILL BREAK THIS CODE
    // if you end up clicking delete and replace the event system game object.

    void Awake()
    {
        virtualMousePos.x = Input.mousePosition.x;
        virtualMousePos.y = Input.mousePosition.y;

        updatePos = false;
        isPressed = false;

        rC = GetComponent<GraphicRaycaster>(); // the canvas
        EventSystem.current.SetSelectedGameObject(null);
    }

    void Update()
    {
        if (Gamepad.current != null)
        {
            moveVirtualMouse();
            UIclickVirtualMouse();
        }
    }

    void UIclickVirtualMouse()
    {
        ptrEventData = new PointerEventData(eventSys);
        ptrEventData.position = Input.mousePosition;

        results = new List<RaycastResult>();

        rC.Raycast(ptrEventData, results);

        if (isPressed == false)
        {
            foreach (RaycastResult result in results)
            {
                if (Gamepad.current.aButton.isPressed)
                {
                    ExecuteEvents.Execute(result.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
                    EventSystem.current.SetSelectedGameObject(null);
                    isPressed = true;
                }
            }
        }

        if(!Gamepad.current.aButton.isPressed)
        {
            isPressed = false;
        }
        
    }

    void moveVirtualMouse()
    {
        updatePos = false; // reset to allow for mouse movement again

        if (Gamepad.current.rightStick.ReadValue().x <= -joystickDetect)
        {
            virtualMousePos.x = Input.mousePosition.x - (virtualMouseSpd * Time.deltaTime * Mathf.Abs(Gamepad.current.rightStick.ReadValue().x));
            updatePos = true;
        }
        else if (Gamepad.current.rightStick.ReadValue().x >= joystickDetect)
        {
            virtualMousePos.x = Input.mousePosition.x + (virtualMouseSpd * Time.deltaTime * Mathf.Abs(Gamepad.current.rightStick.ReadValue().x));
            updatePos = true;
        }

        if (Gamepad.current.rightStick.ReadValue().y <= -joystickDetect)
        {
            virtualMousePos.y = Input.mousePosition.y - (virtualMouseSpd * Time.deltaTime * Mathf.Abs(Gamepad.current.rightStick.ReadValue().y));
            updatePos = true;
        }
        else if (Gamepad.current.rightStick.ReadValue().y >= joystickDetect)
        {
            virtualMousePos.y = Input.mousePosition.y + (virtualMouseSpd * Time.deltaTime * Mathf.Abs(Gamepad.current.rightStick.ReadValue().y));
            updatePos = true;
        }

        if (updatePos)
        {
            Mouse.current.WarpCursorPosition(virtualMousePos);
        }
    }
}

