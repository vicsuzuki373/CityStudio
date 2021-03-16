using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeverageCup : MonoBehaviour
{
    public GameObject Cup1;
    public GameObject Cup2;
    private int random;

    public static float timeDistracted = 0;
    public static float progress;
    public static bool restart;

    void Start()
    {
        progress = 0;
        Cup2.SetActive(false);
    }

    private void Update()
    {
        if (progress > 0)
            progress -= 20 * Time.deltaTime;

        if (restart)
        {
            Restart();
            restart = false;
        }
    }

    private void OnMouseOver()
    {
        if (!MenuController.paused)
        {
            timeDistracted += Time.deltaTime;
            if (Cup1.activeSelf)
            {
                GameController.interact = true;
                GameController.interactMessage = "Drink";
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    random = Random.Range(1, 10);
                    if (random > 6)
                    {
                        Cup1.SetActive(false);
                        Cup2.SetActive(true);
                    }
                }
            }
            else
            {
                GameController.interact = true;
                GameController.interactMessage = "Clean up";
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.JoystickButton0))
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

    private void Restart()
    {
        timeDistracted = 0;
        progress = 0;
        Cup2.SetActive(false);
        Cup1.SetActive(true);
    }
}
