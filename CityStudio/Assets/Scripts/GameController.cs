using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject conlinCheckpoint;
    public GameObject LeftLane;
    public GameObject RightLane;
    public Text objectiveText;
    public GameObject wrongLaneUI;
    public static bool wrongLane = false;
    private float wrongLaneTimer = 0;

    public GameObject redLightUI;
    public static bool redLight = false;
    private float redLightTimer = 0;

    public GameObject speedLimitUI;
    public int speedLimit = 50;

    public GameObject EtoInteract;
    public GameObject cupProgress;
    public Text speedometer;

    public static int cupinteract = 0;

    private int intProgress;
    private float speed;
    private float speedint;

    void Start()
    {
        wrongLaneUI.SetActive(false);
        redLightUI.SetActive(false);
        speedLimitUI.SetActive(false);
    }

    void Update()
    {
        //Checkpoint updates
        if (conlinCheckpoint.activeSelf)
        {
            LeftLane.SetActive(true);
            RightLane.SetActive(false);
        }
        else
        {
            objectiveText.text = "Drive to Rossland Road";
            LeftLane.SetActive(false);
            RightLane.SetActive(true);
        }

        //Check if player is in wrong lane
        if (wrongLane && wrongLaneTimer < 0.5f)
        {
            wrongLaneUI.SetActive(true);
            wrongLaneTimer += Time.deltaTime;
        }
        else
        {
            wrongLaneUI.SetActive(false);
            wrongLaneTimer = 0;
            wrongLane = false;
        }

        //If player passed through red light
        if (redLight && redLightTimer < 1)
        {
            redLightUI.SetActive(true);
            redLightTimer += Time.deltaTime;
        }
        else
        {
            redLightUI.SetActive(false);
            redLightTimer = 0;
            redLight = false;
        }

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

        speed = Car.velocity * 14;
        speedint = (int)speed;
        speedometer.text = "Speed: " + speedint.ToString();
        if (speedint > speedLimit)
            speedLimitUI.SetActive(true);
        else
            speedLimitUI.SetActive(false);


    }
}
