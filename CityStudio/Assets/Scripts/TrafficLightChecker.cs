using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightChecker : MonoBehaviour
{
    private float delay = 0;
    public static int amount = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        delay += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "car" && TrafficLightController.status > 2 && delay > 5)
        {
            GameController.redLight = true;
            amount += 1;
            delay = 0;
        }
    }
}
