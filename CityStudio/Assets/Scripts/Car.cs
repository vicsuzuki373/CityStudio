using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float maxspeed = 5;
    public float steer = 25;
    public float maxsteer = 50;
    public float acceleration = 15;

    public static bool col = false;
    public static float velocity;

    private float rotate;
    private float accelerate;
    private bool reverse = false;

    public GameObject editorCam;

    void Start()
    {
    }

    void Update()
    {
        if (editorCam.activeInHierarchy) // disable car control during editor
            return;

        velocity = gameObject.GetComponent<Rigidbody>().velocity.magnitude;

        if (Input.GetKeyUp(KeyCode.Q) && velocity < 0.1f)
            reverse = !reverse;
        if (Input.GetKey(KeyCode.W) && velocity < maxspeed)
            accelerate = acceleration;
        if (Input.GetKey(KeyCode.S) && velocity > 0.2f)
            accelerate = -acceleration * 2;
        if (Input.GetKey(KeyCode.A) && velocity > 0.01f && rotate > -maxsteer)
            rotate -= steer * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) && velocity > 0.01f && rotate < maxsteer)
            rotate += steer * Time.deltaTime;


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

        accelerate = 0;
        rotate *= 0.9f;
        if (velocity < 0.01f)
            velocity = 0;
    }
}