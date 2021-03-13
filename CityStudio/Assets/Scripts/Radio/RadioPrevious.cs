using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioPrevious : MonoBehaviour
{
    private void OnMouseOver()
    {
        GameController.interact = true;
        GameController.interactMessage = "Previous Song";
        if (Input.GetMouseButtonDown(0))
            Radio.action = 1;
    }
}
