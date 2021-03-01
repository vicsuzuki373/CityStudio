using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    public Text speedometer;
    public float maxspeed = 5;
    public float steer = 0.3f;
    public float acceleration = 15;

    static public bool col = false;

    private float rotate;
    private float accelerate;
    private float velocity;
    private bool reverse = false;

    void Start()
    {
    }

    void Update()
    {
        velocity = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        speedometer.text = "Speed: " + velocity.ToString();

        if (Input.GetKeyUp(KeyCode.Q))
            reverse = !reverse;
        if (Input.GetKey(KeyCode.W) && velocity < maxspeed)
            accelerate = acceleration;
        if (Input.GetKey(KeyCode.S) && velocity > 0)
            gameObject.GetComponent<Rigidbody>().velocity *= 0.99f;
        if (Input.GetKey(KeyCode.A) && velocity > 0.001f)
            rotate = -steer;
        if (Input.GetKey(KeyCode.D) && velocity > 0.01f)
            rotate = steer;

        if (reverse)
        {
            accelerate = accelerate * -1;
            if (rotate != 0)
            {
                rotate *= -1;
                gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity.magnitude * -transform.forward;
                gameObject.GetComponent<Rigidbody>().MoveRotation(transform.rotation * Quaternion.Euler(Vector3.up * rotate));
            }

        }
        else if (rotate != 0)
        {
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity.magnitude * transform.forward;
            gameObject.GetComponent<Rigidbody>().MoveRotation(transform.rotation * Quaternion.Euler(Vector3.up * rotate));
        }
        
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * accelerate * Time.deltaTime, ForceMode.Impulse);

        accelerate = 0;
        rotate = 0;
    }
}