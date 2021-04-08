using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float steer = 10;
    public float acceleration = 1.5f;

    public static float maxspeed = 10;
    public static bool reverse = false;
    public static float velocity;
    public static int amountcollided = 0;
    public static bool restart;
    public static float DistanceDriven = 0;
    public static float sessionTime = 0;

    private bool grounded = true;
    private float distanceTimer = 0;
    private float speedTimer = 0;
    private static float speedCount = 0;
    private static float TotalSpeed = 0;
    private Vector3 lastposition;
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
        lastposition = startPosition;
    }

    void Update()
    {
        float LeftJoystick = Input.GetAxis("LeftJoystick");
        float RT = Input.GetAxis("RT");
        float LT = Input.GetAxis("LT");
        velocity = gameObject.GetComponent<Rigidbody>().velocity.magnitude;

        if (!MenuController.paused)
        {
            if (grounded)
            {
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
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

                if (velocity > 0.01f && rotate > -steer && rotate < steer) // Controller
                {
                    if (LeftJoystick != 0)
                        rotate += steer * Time.deltaTime * LeftJoystick;
                    if (Input.GetKey(KeyCode.A))
                        rotate -= steer * Time.deltaTime;
                    if (Input.GetKey(KeyCode.D))
                        rotate += steer * Time.deltaTime;
                }
            }
            else
            {
                gameObject.GetComponent<Rigidbody>().AddForce(-Vector3.up * Time.deltaTime * 10, ForceMode.Impulse);
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
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
            rotate -= rotate * 8 * Time.deltaTime;

            collisiondelay += Time.deltaTime;

            distanceTimer += Time.deltaTime;

            sessionTime += Time.deltaTime;

            if (distanceTimer > 2)
            {
                DistanceDriven += Vector3.Distance(lastposition, transform.position);
                lastposition = transform.position;
                distanceTimer = 0;
            }

            speedTimer += Time.deltaTime;
            if(speedTimer > 1)
            {
                TotalSpeed += velocity;
                speedCount += 1;
                speedTimer = 0;
            }

            if (restart)
            {
                Restart();
                restart = false;
            }
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
        if (collision.collider.name == "colliderMesh")
            grounded = true;
        else if (collision.collider.name == "resetcollider")
        {
            transform.Translate(Vector3.up);
            transform.rotation = startRotation;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.name == "colliderMesh")
            grounded = false;
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
        DistanceDriven = 0;
        grounded = true;
        distanceTimer = 0;
        speedTimer = 0;
        speedCount = 0;
        TotalSpeed = 0;
        sessionTime = 0;
        lastposition = startPosition;
    }

    public static int GetAverageSpeed()
    {
        return (int)(10 * (TotalSpeed / speedCount));
    }
}