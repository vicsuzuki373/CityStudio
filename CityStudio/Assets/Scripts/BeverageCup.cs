using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeverageCup : MonoBehaviour
{
    public GameObject Cup1;
    public GameObject Cup2;
    private int random;
    public static float progress;

    void Start()
    {
        progress = 0;
        Cup2.SetActive(false);
    }

    private void Update()
    {
        if (progress > 0)
            progress -= 20 * Time.deltaTime;

    }

    private void OnMouseOver()
    {
        if (Cup1.activeSelf)
        {
            GameController.cupinteract = 1;
            if (Input.GetMouseButtonDown(0))
            {
                random = Random.Range(0, 10);
                if (random > 8)
                {
                    Cup1.SetActive(false);
                    Cup2.SetActive(true);
                }
            }
        }
        else
        {
            GameController.cupinteract = 2;
            if (Input.GetMouseButton(0))
            {
                progress += 60 * Time.deltaTime;
                if (progress > 100)
                {
                    progress = 0;
                    Cup1.SetActive(true);
                    Cup2.SetActive(false);
                }
            }
        }
    }
}
