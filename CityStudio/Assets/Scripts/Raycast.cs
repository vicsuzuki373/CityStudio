using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    private Collider target;
    private bool hovered = false;


    void Update()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 10))
        {
            if (!hovered)
            {
                hit.collider.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
                target = hit.collider;
            }
            hovered = true;
        }
        else
        {
            if (hovered)
                target.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
            hovered = false;
        }

        if (hovered)
        {
            hit.collider.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);
        }
    }
}