using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float steer = 25;
    public float maxsteer = 50;
    public float acceleration = 15;

    public static float maxspeed = 10;
    public static bool reverse = false;
    public static float velocity;
    public static int amountcollided = 0;
    public static bool restart;

    private float rotate;
    private float accelerate;
    private float collisiondelay;
    private Vector3 startPosition;
    private Quaternion startRotation;

    public GameObject editorCam;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
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

            if (Input.GetKey(KeyCode.W) && velocity < maxspeed + 0.05f)
                accelerate = acceleration;
            else if (RT > 0 && velocity < maxspeed + 0.05f) // Controller
                accelerate = acceleration * RT;

            if (Input.GetKey(KeyCode.S) && velocity > 0.2f)
                accelerate = -acceleration * 2;
            else if (LT > 0 && velocity > 0.2f) // Controller
                accelerate = -acceleration * 2 * LT;

            if (velocity > 0.01f && rotate > -maxsteer) // Controller
            {
                if(LeftJoystick != 0)
                    rotate += steer * Time.deltaTime * LeftJoystick;
                if (Input.GetKey(KeyCode.A))
                    rotate -= steer * Time.deltaTime;
                if (Input.GetKey(KeyCode.D))
                    rotate += steer * Time.deltaTime;
            }
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

        accelerate = 0;
        rotate *= 0.9f;

        collisiondelay += Time.deltaTime;

        if (restart)
        {
            Restart();
            restart = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name != "colliderMesh" && collisiondelay > 0.5f)
        {
            GameController.collision = true;
            amountcollided += 1;
            collisiondelay = 0;
        }
    }

    private void Restart()
    {
        rotate = 0;
        accelerate = 0;
        collisiondelay = 0;
        reverse = false;
        velocity = 0;
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        amountcollided = 0;
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}