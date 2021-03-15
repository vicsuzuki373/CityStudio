using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioPrevious : MonoBehaviour
{
    private void OnMouseOver()
    {
        if (!MenuController.paused)
        {
            Radio.timeDistracted += Time.deltaTime;
            GameController.interact = true;
            GameController.interactMessage = "Previous Song";
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton0))
                Radio.action = 1;
        }
    }
}
