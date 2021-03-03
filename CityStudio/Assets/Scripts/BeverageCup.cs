using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeverageCup : MonoBehaviour
{
    public GameObject Cup1;
    public GameObject Cup2;
    public GameObject car;
    private int random;
    public static float progress;
    
    void Start()
    {
        progress = 0;
        Cup2.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && Cup1.activeSelf && Car.velocity > 1)
        {
            random = Random.Range(0, 10);
            if (random < 1)
            {
                Cup1.SetActive(false);
                Cup2.SetActive(true);
            }
        }
        if (progress > 0)
            progress -= 20 * Time.deltaTime;


        transform.position = car.gameObject.transform.Find("CupPosition").position;
        transform.rotation = car.gameObject.transform.Find("CupPosition").rotation;
    }

    private void OnMouseEnter()
    {
        if(Cup2.activeSelf)
            UIController.interact = true;
    }

    private void OnMouseOver()
    {
        if (Cup2.activeSelf && Input.GetKey(KeyCode.E))
        {
            progress += 60 * Time.deltaTime;
            if(progress > 100)
            {
                progress = 0;
                Cup2.SetActive(false);
                Cup1.SetActive(false);
            }
        }
    }

    private void OnMouseExit()
    {
        UIController.interact = false;
    }
}
