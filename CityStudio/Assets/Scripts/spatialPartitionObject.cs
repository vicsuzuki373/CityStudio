using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spatialPartitionObject : MonoBehaviour
{
    public spatialPartition controller;
    private bool sent;

    void Start()
    {
        sent = false;
    }

    void Update()
    {
        if(sent == true)
        {
            return;
        }
        else if (controller.regionReady == true)
        {
            controller.sendStaticPosition(gameObject.transform.position, this.gameObject);
            sent = true;
        }
    }
}
