using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioPlayorPause : MonoBehaviour
{
    private void OnMouseOver()
    {
        if (!MenuController.paused)
        {
            Radio.timeDistracted += Time.deltaTime;
            GameController.interact = true;
            GameController.interactMessage = "Play/Pause";
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton0))
                Radio.action = 2;
        }
    }
}
