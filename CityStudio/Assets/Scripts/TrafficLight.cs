using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public bool opposite = false;
    public bool pedestrian = false;
    private GameObject green;
    private GameObject yellow;
    private GameObject red;

    public intersection pedestrians;
    public int pedestrianRoadDirection;

    // Start is called before the first frame update
    void Start()
    {
        if (!pedestrian)
        {
            green = gameObject.transform.Find("Green").gameObject;
            yellow = gameObject.transform.Find("Yellow").gameObject;
            red = gameObject.transform.Find("Red").gameObject;
        }
        else
        {
            green = gameObject.transform.Find("Green").gameObject;
            red = gameObject.transform.Find("Red").gameObject;
        }
        if (opposite)
            red.SetActive(true);
    }

    void activatePedestrianCrossing(int _pedestrianRoadDirection)
    {
        if (green.activeSelf && pedestrians != null)
            pedestrians.toggleCrossWalk(_pedestrianRoadDirection, true);
        else if(pedestrians != null)
            pedestrians.toggleCrossWalk(_pedestrianRoadDirection, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!opposite)
        {
            if (!pedestrian)
                switch (TrafficLightController.status)
                {
                    case 1:
                        red.SetActive(false);
                        yellow.SetActive(false);
                        green.SetActive(true);
                        break;
                    case 2:
                        red.SetActive(false);
                        yellow.SetActive(true);
                        green.SetActive(false);
                        break;
                    case 3:
                        red.SetActive(true);
                        yellow.SetActive(false);
                        green.SetActive(false);
                        break;
                }
            else
                switch (TrafficLightController.status)
                {
                    case 1:
                        red.SetActive(false);
                        green.SetActive(true);
                        break;
                    case 3:
                        red.SetActive(true);
                        green.SetActive(false);
                        break;
                }
        }
        else
        {
            if (!pedestrian)
                switch (TrafficLightController.status)
                {
                    case 4:
                        red.SetActive(false);
                        yellow.SetActive(false);
                        green.SetActive(true);
                        break;
                    case 5:
                        red.SetActive(false);
                        yellow.SetActive(true);
                        green.SetActive(false);
                        break;
                    case 6:
                        red.SetActive(true);
                        yellow.SetActive(false);
                        green.SetActive(false);
                        break;
                }
            else
                switch (TrafficLightController.status)
                {
                    case 4:
                        red.SetActive(false);
                        green.SetActive(true);
                        break;
                    case 6:
                        red.SetActive(true);
                        green.SetActive(false);
                        break;
                }
        }

        activatePedestrianCrossing(pedestrianRoadDirection);
    }

}
