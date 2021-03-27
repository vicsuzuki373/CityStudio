using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class pedestrians : MonoBehaviour
{
    [Header("Pool Settings")]
    public GameObject prefabPedestrian;
    public int poolAmount;
    public List<GameObject> pedestrianPool;
    public static bool restart;

    [Header("Pedestrian Settings")]
    public float maxPedestrianSpeed;
    public float distanceToSpawn;

    void Start()
    {
        for(int i = 0; i < poolAmount; i++)
        {
            createPedestrian();
        }

        restart = false;
    }
    void Update()
    {
        if(restart)
        {
            foreach(pedestrianObject child in gameObject.GetComponentsInChildren<pedestrianObject>())
            {
                child.restart();
            }
            restart = false;
        }
    }

    void createPedestrian()
    {
        GameObject prefabHandle;
        prefabHandle = Instantiate(prefabPedestrian);
        prefabHandle.gameObject.transform.parent = this.transform;

        prefabHandle.GetComponent<NavMeshAgent>().avoidancePriority = Random.Range(0, 100);
        prefabHandle.GetComponent<NavMeshAgent>().speed = Random.Range(0.6f, maxPedestrianSpeed);

        pedestrianPool.Add(prefabHandle);
        prefabHandle.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        prefabHandle.SetActive(false);
    }

    public GameObject getPedestrian(Vector3 location)
    {
        GameObject pedestrianHandle;

        if (!(pedestrianPool.Count >= 1.0f))
        {
            createPedestrian();
        }

        pedestrianHandle = pedestrianPool[pedestrianPool.Count - 1];
        pedestrianPool.RemoveAt(pedestrianPool.Count - 1);

        pedestrianHandle.transform.position = location;
        pedestrianHandle.SetActive(true); // set transform and destination?

        return pedestrianHandle;
    }

    public void stashPedestrian(GameObject pedestrianHandle)
    {
        pedestrianHandle.SetActive(false); //reset transform and destination?
        pedestrianPool.Add(pedestrianHandle);
    }

    
}
