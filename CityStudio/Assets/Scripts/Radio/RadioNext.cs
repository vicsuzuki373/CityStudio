using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioNext : MonoBehaviour
{
    private void OnMouseOver()
    {
        GameController.interact = true;
        GameController.interactMessage = "Next Song";
        if (Input.GetMouseButtonDown(0))
            Radio.action = 3;
    }
}
