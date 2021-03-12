using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    private float timer;
    public static int status;
    // Start is called before the first frame update
    void Start()
    {
        status = 3;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 30)
            timer = 0;

        if (timer < 10) //10 sec green
            status = 1;
        else if (timer < 13) //3 sec yellow
            status = 2;
        else if (timer < 15) //2 sec red for all
            status = 3;
        else if (timer < 25) //10 sec green for opposite
            status = 4;
        else if (timer < 28) //3 sec green for opposite
            status = 5;
        else if (timer < 30) //2 red for all
            status = 6;
    }
}
