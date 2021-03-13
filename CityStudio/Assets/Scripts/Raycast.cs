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

    void Start()
    {
        worldVolume.profile.TryGet(out worldDepth);
    }

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

        



        if(Vector3.Distance(transform.position, hit.point) < 0.75f)
        {
            worldDepth.focalLength.value = Mathf.Lerp(worldDepth.focalLength.value, 25.0f, Time.deltaTime * focalShiftSpeed);
            worldDepth.focusDistance.value = Mathf.Lerp(worldDepth.focusDistance.value, 0.2f, Time.deltaTime * focalShiftSpeed);
            //Debug.Log("near");

        } //near
        else
        {
            worldDepth.focalLength.value = Mathf.Lerp(worldDepth.focalLength.value, 20.0f, Time.deltaTime * focalShiftSpeed);
            worldDepth.focusDistance.value = Mathf.Lerp(worldDepth.focusDistance.value, 15f, Time.deltaTime * focalShiftSpeed);
            //Debug.Log("far");
        } //far
   
        //Debug.Log(Vector3.Distance(transform.position, hit.point));
    }
}