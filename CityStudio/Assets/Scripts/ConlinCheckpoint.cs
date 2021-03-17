using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConlinCheckpoint : MonoBehaviour
{
    public GameObject RosslandCheckpoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "car")
        {
            RosslandCheckpoint.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
