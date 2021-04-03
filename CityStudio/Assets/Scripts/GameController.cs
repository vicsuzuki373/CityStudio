using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Objectives")]
    public GameObject conlinCheckpoint;
    public GameObject rosslandCheckpoint;
    public GameObject LeftLane;
    public GameObject RightLane;
    public GameObject Objective;

    [Header("Infractions")]
    public GameObject wrongLaneUI;
    public static bool wrongLane = false;
    private float wrongLaneTimer = 1;

    public GameObject redLightUI;
    public static bool redLight = false;
    private float redLightTimer = 0;

    public GameObject speedLimitUI;
    public int speedLimit = 50;
    private float lerpvalue = 0;

    public GameObject collisionUI;
    public static bool collision = false;
    private float collisionTimer = 0;

    [Header("UI Gameplay Elements")]
    public GameObject Interact;
    public GameObject cupProgress;
    public Text speedometer;
    public Text Gear;
    public GameObject speedometerPointer;

    public static bool interact = false;
    public static string interactMessage = "Interact";
    public static float amountOverSpeed = 0;
    public static bool restart;

    private int intProgress;
    private float speed;
    private float speedint;

    void Start()
    {
        wrongLaneUI.SetActive(false);
        redLightUI.SetActive(false);
        speedLimitUI.SetActive(false);
        collisionUI.SetActive(false);


        //cap framerate to lessen stutter
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        //Speedometer
        speed = Car.velocity * 10;
        speedint = (int)speed;
        speedometer.text = speedint.ToString(); //Number
        lerpvalue = Car.velocity / Car.maxspeed;
        speedometerPointer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(122, -119, lerpvalue))); //Pointer rotation
        if (Car.reverse)
            Gear.text = "R";
        else
            Gear.text = "D";
        if (speedint > speedLimit) // Over speed limit
        {
            speedLimitUI.SetActive(true);
            amountOverSpeed += Time.deltaTime;
        }
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
            cupProgress.transform.GetChild(0).localScale = new Vector3(BeverageCup.progress / 100, 1, 1);
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
            Objective.GetComponentInChildren<Text>().text = "Drive to Rossland Road";
            LeftLane.SetActive(false);
            RightLane.SetActive(true);
        }

        //Check if player is in wrong lane
        if (!wrongLane)
        {
            if (wrongLaneTimer > 0.1f)
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

        //If player collided with objects
        if (collision && collisionTimer < 1)
        {
            collisionUI.SetActive(true);
            collisionTimer += Time.deltaTime;
        }
        else
        {
            collisionUI.SetActive(false);
            collisionTimer = 0;
            collision = false;
        }

        if (restart)
        {
            Restart();
            restart = false;
        }
    }

    private void Restart()
    {
        wrongLaneUI.SetActive(false);
        redLightUI.SetActive(false);
        speedLimitUI.SetActive(false);
        collisionUI.SetActive(false);
        conlinCheckpoint.SetActive(true);
        rosslandCheckpoint.SetActive(false);
        Objective.GetComponentInChildren<Text>().text = "Drive to Conlin Road";
        wrongLane = false;
        wrongLaneTimer = 1;
        redLight = false;
        redLightTimer = 0;
        lerpvalue = 0;
        collision = false;
        collisionTimer = 0;
        amountOverSpeed = 0;
        cupProgress.gameObject.SetActive(false);
        interact = false;
    }
}
