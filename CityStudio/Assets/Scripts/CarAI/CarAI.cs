using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    public bool restart = false;
    public static int maxamountcar = 1000;
    public bool opposite = false;
    public float speed = 3;
    public GameObject playercar;

    public List<GameObject> cars = new List<GameObject>();
    public static int amountcar = 0;
    private List<GameObject> waypoints = new List<GameObject>();

    private List<GameObject> spawnedcars = new List<GameObject>();
    private List<int> nextwaypoint = new List<int>();
    private int random = 0;
    private float timer = 0;
    private float spawndelay = 10;

    // Start is called before the first frame update
    void Start()
    {
        random = Random.Range(0, cars.Count);
        spawndelay = Random.Range(5, 10);
        spawnedcars.Add((GameObject)Instantiate(cars[random], transform.position, transform.rotation));
        amountcar += 1;
        nextwaypoint.Add(0);
        for (int i = 0; i < transform.childCount; i++)
        {
            waypoints.Add(transform.GetChild(i).gameObject);
            if(transform.GetChild(i).gameObject.GetComponent<CarAIWaypoint>().spawncar)
            {
                random = Random.Range(0, cars.Count);
                spawnedcars.Add((GameObject)Instantiate(cars[random], transform.GetChild(i).position, transform.rotation));
                amountcar += 1;
                nextwaypoint.Add(i + 1);
            }
        }
        for(int i = 0; i < spawnedcars.Count; i++)
            spawnedcars[i].transform.LookAt(waypoints[nextwaypoint[i]].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!MenuController.paused)
        {
            timer += Time.deltaTime;
            if (timer > spawndelay && Vector3.Distance(spawnedcars[spawnedcars.Count - 1].transform.position, transform.position) > 5 && amountcar < maxamountcar)
            {
                random = Random.Range(0, cars.Count);
                spawndelay = Random.Range(5, 10);
                spawnedcars.Add((GameObject)Instantiate(cars[random], transform.position, transform.rotation));
                amountcar += 1;
                nextwaypoint.Add(0);
                timer = 0;
                spawnedcars[spawnedcars.Count - 1].transform.LookAt(waypoints[nextwaypoint[spawnedcars.Count - 1]].transform.position);
            }

            for (int i = 0; i < spawnedcars.Count; i++)
            {
                if (Vector3.Distance(spawnedcars[i].transform.position, playercar.transform.position) > 100)
                {
                    foreach (Renderer r in spawnedcars[i].GetComponentsInChildren<Renderer>())
                        r.enabled = false;
                }
                else
                {
                    foreach (Renderer r in spawnedcars[i].GetComponentsInChildren<Renderer>())
                        r.enabled = true;
                }

                if (!spawnedcars[i].GetComponentInChildren<CarAICollision>().stop && !spawnedcars[i].GetComponentInChildren<CarAICollision>().forcestop)
                    spawnedcars[i].transform.Translate(Vector3.forward * Time.deltaTime * speed);

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
        if (restart)
        {
            Restart();
            restart = false;
        }
    }

    private void Restart()
    {
        random = Random.Range(0, cars.Count);
        for (int i = 0; i < spawnedcars.Count; i++)
        {
            GameObject temp = spawnedcars[i];
            Destroy(temp);
        }
        spawnedcars.Clear();
        spawnedcars.Add((GameObject)Instantiate(cars[random], transform.position, transform.rotation));
        amountcar += 1;
        nextwaypoint.Clear();
        nextwaypoint.Add(0);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.GetComponent<CarAIWaypoint>().spawncar)
            {
                random = Random.Range(0, cars.Count);
                spawnedcars.Add((GameObject)Instantiate(cars[random], transform.GetChild(i).position, transform.rotation));
                amountcar += 1;
                nextwaypoint.Add(i + 1);
            }
        }
        for (int i = 0; i < spawnedcars.Count; i++)
            spawnedcars[i].transform.LookAt(waypoints[nextwaypoint[i]].transform.position);
        timer = 0;
    }
}
