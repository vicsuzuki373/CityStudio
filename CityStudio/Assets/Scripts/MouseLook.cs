using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    Vector2 rotation = new Vector2(0, 0);
    float sensitivity = 3;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!MenuController.paused)
        {
            rotation.y += Input.GetAxis("Mouse X") + Input.GetAxis("RightJoystickY") / 3;
            rotation.x += -Input.GetAxis("Mouse Y") + Input.GetAxis("RightJoystickX") / 5;
            rotation.x = Mathf.Clamp(rotation.x, -8f, 20f);
            rotation.y = Mathf.Clamp(rotation.y, -25f, 25f);

            transform.localEulerAngles = (Vector2)rotation * sensitivity;
        }
    }
}
