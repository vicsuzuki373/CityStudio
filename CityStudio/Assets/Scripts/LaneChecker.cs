using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneChecker : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody)
        {
            GameController.wrongLane = true;
        }
    }
}
