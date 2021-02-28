using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    public Text speedometer;

    private float speed = 0;
    private float maxspeed = 20;
    private bool reverse = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        speedometer.text = "Speed: " + speed.ToString();
        
        if (Input.GetKeyUp(KeyCode.Q))
        {
            reverse = !reverse;
        }
        if (Input.GetKey(KeyCode.W) && speed < maxspeed)
        {
            if(!reverse)
                speed += 0.03f;
            else
                speed -= 0.03f;
        }
        if (speed > 0)
        {
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(new Vector3(0, -0.08f, 0));
            if (Input.GetKey(KeyCode.D))
                transform.Rotate(new Vector3(0, 0.08f, 0));
            if (Input.GetKey(KeyCode.S))
                speed -= 0.07f;
            speed -= 0.01f;
            if (speed < 0)
                speed = 0;
        }
        else if (speed < 0)
        {
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(new Vector3(0, 0.08f, 0));
            if (Input.GetKey(KeyCode.D))
                transform.Rotate(new Vector3(0, -0.08f, 0));
            if (Input.GetKey(KeyCode.S))
                speed += 0.07f;
            speed += 0.01f;
            if (speed > 0)
                speed = 0;
        }

        gameObject.GetComponent<Rigidbody>().MovePosition(transform.position + (transform.forward * speed * Time.deltaTime));
    }
}