using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneChecker : MonoBehaviour
{
    public static float amount = 0;
    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody)
        {
            GameController.wrongLane = true;
            amount += Time.deltaTime;
        }
    }
}
