using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float steer = 25;
    public float maxsteer = 50;
    public float acceleration = 15;

    public static float maxspeed = 10;
    public static bool col = false;
    public static bool reverse = false;
    public static float velocity;

    private float rotate;
    private float accelerate;

    public GameObject editorCam;

    void Start()
    {
    }

    void Update()
    {
        //if (editorCam.activeInHierarchy) // disable car control during editor
        //    return;

        float LeftJoystick = Input.GetAxis("LeftJoystick");
        float RT = Input.GetAxis("RT");
        float LT = Input.GetAxis("LT");
        velocity = gameObject.GetComponent<Rigidbody>().velocity.magnitude;

        if (!MenuController.paused)
        {
            if ((Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.JoystickButton4)) && velocity < 0.1f)
                reverse = true;
            else if ((Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.JoystickButton5)) && velocity < 0.1f)
                reverse = false;

            if (Input.GetKey(KeyCode.W) && velocity < maxspeed)
                accelerate = acceleration;
            else if (RT > 0 && velocity < maxspeed) // Controller
                accelerate = acceleration * RT;

            if (Input.GetKey(KeyCode.S) && velocity > 0.2f)
                accelerate = -acceleration * 2;
            else if (LT > 0 && velocity > 0.2f) // Controller
                accelerate = -acceleration * 2 * LT;

            if (Input.GetKey(KeyCode.A) && velocity > 0.01f && rotate > -maxsteer)
                rotate -= steer * Time.deltaTime;
            if (Input.GetKey(KeyCode.D) && velocity > 0.01f && rotate < maxsteer)
                rotate += steer * Time.deltaTime;
            if (velocity > 0.01f && rotate > -maxsteer) // Controller
                rotate += steer * 1.5f * Time.deltaTime * LeftJoystick;
        }

        if (!reverse)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * accelerate * Time.deltaTime, ForceMode.Impulse);
            if (rotate != 0)
            {
                gameObject.GetComponent<Rigidbody>().MoveRotation(transform.rotation * Quaternion.Euler(transform.up * rotate));
                gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity.magnitude * transform.forward;
            }

        }
        else
        {
            gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * -accelerate * Time.deltaTime, ForceMode.Impulse);
            if (rotate != 0)
            {
                gameObject.GetComponent<Rigidbody>().MoveRotation(transform.rotation * Quaternion.Euler(transform.up * rotate * -1));
                gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity.magnitude * -transform.forward;
            }
        }

        if (velocity < 0.005f && accelerate == 0)
            velocity = 0;
        accelerate = 0;
        rotate *= 0.9f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name != "colliderMesh")
            GameController.collision = true;
    }
}