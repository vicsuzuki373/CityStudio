using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject EtoInteract;
    public GameObject cupProgress;
    public Text speedometer;

    public static int cupinteract = 0;

    private int intProgress;


    // Update is called once per frame
    void Update()
    {
        if (cupinteract != 0)
            EtoInteract.SetActive(true);
        else
            EtoInteract.SetActive(false);

        cupProgress.gameObject.SetActive(false);

        if (cupinteract == 1)
            EtoInteract.GetComponentInChildren<Text>().text = "Drink";
        else if (cupinteract == 2)
        {
            EtoInteract.GetComponentInChildren<Text>().text = "Fix";
            if (BeverageCup.progress > 0)
            {
                cupProgress.gameObject.SetActive(true);
                intProgress = (int)BeverageCup.progress;
                cupProgress.GetComponentInChildren<Text>().text = intProgress.ToString();
            }
        }
        else if (cupinteract == 3)
            EtoInteract.GetComponentInChildren<Text>().text = "Turn Radio On/Off";

        cupinteract = 0;


        speedometer.text = "Speed: " + Car.velocity.ToString();
    }
}
