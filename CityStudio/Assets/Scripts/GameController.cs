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
    private float wrongLaneTimer = 1;

    public GameObject redLightUI;
    public static bool redLight = false;
    private float redLightTimer = 0;

    public GameObject speedLimitUI;
    public int speedLimit = 50;

    public GameObject Interact;
    public GameObject cupProgress;
    public Text speedometer;

    public static bool interact = false;
    public static string interactMessage = "E to Interact";

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
        //Speedometer
        speed = Car.velocity * 14;
        speedint = (int)speed;
        speedometer.text = "Speed: " + speedint.ToString();
        if (speedint > speedLimit)
            speedLimitUI.SetActive(true);
        else
            speedLimitUI.SetActive(false);

        //Show message to interact
        if (interact)
        {
            Interact.GetComponentInChildren<Text>().text = interactMessage;
            Interact.SetActive(true);
        }
        else
            Interact.SetActive(false);

        //Show progress only when it's above 0
        cupProgress.gameObject.SetActive(false);
        if (BeverageCup.progress > 0 && interact && interactMessage == "Clean up")
        {
            cupProgress.gameObject.SetActive(true);
            intProgress = (int)BeverageCup.progress;
            cupProgress.GetComponentInChildren<Text>().text = intProgress.ToString();
        }
        interact = false;
               
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
        if (!wrongLane)
        {
            if(wrongLaneTimer > 0.2f)
                wrongLaneUI.SetActive(false);
        }
        else
        {
            wrongLaneUI.SetActive(true);
            wrongLaneTimer = 0;
        }
        wrongLaneTimer += Time.deltaTime;
        wrongLane = false;

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
    }
}
