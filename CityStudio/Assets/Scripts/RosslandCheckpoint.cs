using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosslandCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "car")
        {
            MenuController.gameover = true;
            gameObject.SetActive(false);
        }
    }
}
