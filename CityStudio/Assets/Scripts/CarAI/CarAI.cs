using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    public static float spawndelay = 10;
    public static int amountcar = 0;
    public static bool restart = false;
    public bool opposite = false;

    public List<GameObject> cars = new List<GameObject>();
    public List<GameObject> waypoints = new List<GameObject>();

    private List<GameObject> spawnedcars = new List<GameObject>();
    private List<int> nextwaypoint = new List<int>();
    private int random = 0;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        random = Random.Range(0, cars.Count - 1);
        spawnedcars.Add((GameObject)Instantiate(cars[random], transform.position, transform.rotation));
        amountcar += 1;
        nextwaypoint.Add(0);
        spawnedcars[spawnedcars.Count - 1].transform.LookAt(waypoints[nextwaypoint[spawnedcars.Count - 1]].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!MenuController.paused)
        {
            timer += Time.deltaTime;
            if (timer > spawndelay && Vector3.Distance(spawnedcars[spawnedcars.Count - 1].transform.position, transform.position) > 5)
            {
                random = Random.Range(0, cars.Count - 1);
                spawnedcars.Add((GameObject)Instantiate(cars[random], transform.position, transform.rotation));
                amountcar += 1;
                nextwaypoint.Add(0);
                timer = 0;
                spawnedcars[spawnedcars.Count - 1].transform.LookAt(waypoints[nextwaypoint[spawnedcars.Count - 1]].transform.position);
            }

            for (int i = 0; i < spawnedcars.Count; i++)
            {
                if (!spawnedcars[i].GetComponentInChildren<CarAICollision>().stop && !spawnedcars[i].GetComponentInChildren<CarAICollision>().forcestop)
                    spawnedcars[i].transform.Translate(Vector3.forward * Time.deltaTime * 3);

                if (Vector3.Distance(spawnedcars[i].transform.position, waypoints[nextwaypoint[i]].transform.position) < 0.5f)
                {
                    if (waypoints[nextwaypoint[i]].GetComponent<CarAIWaypoint>().trafficlightwp && !opposite && TrafficLightController.status != 1)
                        spawnedcars[i].GetComponentInChildren<CarAICollision>().forcestop = true;
                    else if (waypoints[nextwaypoint[i]].GetComponent<CarAIWaypoint>().trafficlightwp && opposite && TrafficLightController.status != 4)
                        spawnedcars[i].GetComponentInChildren<CarAICollision>().forcestop = true;
                    else
                    {
                        spawnedcars[i].GetComponentInChildren<CarAICollision>().forcestop = false;
                        nextwaypoint[i] += 1;
                        if (nextwaypoint[i] > waypoints.Count - 1)
                        {
                            Destroy(spawnedcars[i]);
                            spawnedcars.RemoveAt(i);
                            nextwaypoint.RemoveAt(i);
                            amountcar -= 1;
                        }
                        else
                            spawnedcars[i].transform.LookAt(waypoints[nextwaypoint[i]].transform.position);
                    }
                }
            }
        }
        if(restart)
        {
            Restart();
            restart = false;
        }
    }

    private void Restart()
    {
        random = Random.Range(0, cars.Count - 1);
        for (int i = 0; i < spawnedcars.Count; i++)
        {
            Destroy(spawnedcars[i]);
            spawnedcars.RemoveAt(i);
        }
        spawnedcars.Clear();
        spawnedcars.Add((GameObject)Instantiate(cars[random], transform.position, transform.rotation));
        nextwaypoint.Clear();
        nextwaypoint.Add(0);
        amountcar = 1;
        spawnedcars[spawnedcars.Count - 1].transform.LookAt(waypoints[nextwaypoint[spawnedcars.Count - 1]].transform.position);
        timer = 0;
    }
}
