using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Raycast : MonoBehaviour
{
    private Collider target;
    private bool hovered = false;

    public Volume worldVolume;
    DepthOfField worldDepth;
    public float focalShiftSpeed;

    public bool pauseFocalShift = false;

    void Start()
    {
        worldVolume.profile.TryGet(out worldDepth);
    }

    void Update()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = new Ray(gameObject.transform.position, gameObject.transform.forward);

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
            if (hovered && target != null)
                target.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
            hovered = false;
        }

        if (hovered)
        {
            hit.collider.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);
        }




        if (pauseFocalShift == false)
        {
            if (Vector3.Distance(transform.position, hit.point) < 0.75f)
            {
                if(hit.collider.tag != "ignoreRaycast")
                    worldDepth.focusDistance.value = Mathf.Lerp(worldDepth.focusDistance.value, 0.2f, Time.deltaTime * focalShiftSpeed);

            } //near
            else
            {
                worldDepth.focusDistance.value = Mathf.Lerp(worldDepth.focusDistance.value, 15f, Time.deltaTime * focalShiftSpeed);
            } //far
        }
    }
}