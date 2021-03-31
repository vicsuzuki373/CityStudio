using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    Vector2 rotation = new Vector2(0, 0);
    float sensitivity = 3;
    public static bool stop = false;
    private float zoom = 30;
    private float fov = 90;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!MenuController.paused)
        {
            if (!stop)
            {
                rotation.y += Input.GetAxis("Mouse X") + Input.GetAxis("RightJoystickY") / 1.5f;
                rotation.x += -Input.GetAxis("Mouse Y") + Input.GetAxis("RightJoystickX") / 2.5f;
                rotation.x = Mathf.Clamp(rotation.x, -10f, 25f);
                rotation.y = Mathf.Clamp(rotation.y, -38f, 38f);

                transform.localEulerAngles = (Vector2)rotation * sensitivity;

                if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.JoystickButton2))
                {
                    float Keypad = Input.GetAxis("LeftKeypadY");
                    zoom -= Input.GetAxis("Mouse ScrollWheel") * 20 + Keypad;
                    zoom = Mathf.Clamp(zoom, 30, 80);
                    fov -= Time.deltaTime * 200;
                    fov = Mathf.Clamp(fov, zoom, 90);
                    gameObject.GetComponent<Camera>().fieldOfView = fov;

                }
                else
                {
                    fov += Time.deltaTime * 200;
                    fov = Mathf.Clamp(fov, zoom, 90);
                    gameObject.GetComponent<Camera>().fieldOfView = fov;
                }
            }
            else
            {
                fov += Time.deltaTime * 200;
                fov = Mathf.Clamp(fov, zoom, 90);
                gameObject.GetComponent<Camera>().fieldOfView = fov;
            }
        }
    }
}
