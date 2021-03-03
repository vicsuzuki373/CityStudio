using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject EtoInteract;
    public Text cupProgress;
    public Text speedometer;

    public static bool interact = false;

    private int intProgress;


    // Update is called once per frame
    void Update()
    {
        if (interact)
            EtoInteract.SetActive(true);
        else
            EtoInteract.SetActive(false);

        if(BeverageCup.progress > 0)
        {
            cupProgress.gameObject.SetActive(true);
            intProgress = (int)BeverageCup.progress;
            cupProgress.text = intProgress.ToString();
        }
        else
            cupProgress.gameObject.SetActive(false);
        
        speedometer.text = "Speed: " + Car.velocity.ToString();
    }
}
