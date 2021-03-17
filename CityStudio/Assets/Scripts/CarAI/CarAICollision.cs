using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAICollision : MonoBehaviour
{
    public bool stop = false;
    public bool forcestop = false;

    private void Start()
    {
        stop = false;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
            stop = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
            stop = false;
    }
}
