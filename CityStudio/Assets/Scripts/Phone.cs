using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    public static float timeDistracted = 0;
    public static bool restart;
    public GameObject Screen;

    private bool isRinging = false;
    private float PhoneDelay = 0;
    private int random = 0;

    void Start()
    {
        gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<AudioSource>().mute = true;
        Screen.SetActive(false);
    }

    void Update()
    {
        if (!isRinging)
            PhoneDelay += Time.deltaTime;
        if (PhoneDelay > 10)
        {
            random = Random.Range(0, 10);
            if (random > 5)
            {
                gameObject.GetComponent<AudioSource>().mute = false;
                Screen.SetActive(true);
                isRinging = true;
            }
            PhoneDelay = 0;
        }
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
            if (isRinging)
            {
                GameController.interact = true;
                GameController.interactMessage = "Decline Call";
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    gameObject.GetComponent<AudioSource>().mute = true;
                    Screen.SetActive(false);
                    isRinging = false;
                }
            }
        }
    }

    private void Restart()
    {
        gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<AudioSource>().mute = true;
        Screen.SetActive(false);
        timeDistracted = 0;
        PhoneDelay = 0;
        isRinging = false;
    }
}
