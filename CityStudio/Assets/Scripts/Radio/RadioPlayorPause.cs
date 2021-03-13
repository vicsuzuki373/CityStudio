using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioPlayorPause : MonoBehaviour
{
    private void OnMouseOver()
    {
        GameController.interact = true;
        GameController.interactMessage = "Play/Pause";
        if (Input.GetMouseButtonDown(0))
            Radio.action = 2;
    }
}
