using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardDistraction : MonoBehaviour
{
    public float timeDistracted = 0;
    public bool restart;

    void Update()
    {
        if (restart)
        {
            Restart();
            restart = false;
        }
    }

    private void OnMouseOver()
    {
        if (!MenuController.paused)
        {
            timeDistracted += Time.deltaTime;
        }
    }

    private void Restart()
    {
        timeDistracted = 0;
    }
}
