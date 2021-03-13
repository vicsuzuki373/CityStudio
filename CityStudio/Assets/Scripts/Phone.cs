using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
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
        if(!isRinging)
            PhoneDelay += Time.deltaTime;
        if(PhoneDelay > 10)
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
    }

    private void OnMouseOver()
    {
        if (isRinging)
        {
            GameController.interact = true;
            GameController.interactMessage = "Decline Call";
            if (Input.GetMouseButtonDown(0))
            {
                gameObject.GetComponent<AudioSource>().mute = true;
                Screen.SetActive(false);
                isRinging = false;
            }
        }
    }
}
