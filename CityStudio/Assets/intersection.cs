using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intersection : MonoBehaviour
{
    public List<GameObject> corners;
    public int maxSpawnPerCorner;
    float minX, maxX, minZ, maxZ;
    public pedestrians pool;
    public GameObject player;
    public bool playerNearby = false;

    float getDistanceToPlayer()
    {
        return Vector3.Distance(player.transform.position, gameObject.transform.position);
    }

    public Vector3 sendPedestrianTarget()
    {
        GameObject target = corners[Random.Range(0, corners.Count - 1)];

        return new Vector3(
            Random.Range(target.transform.position.x - target.GetComponent<Collider>().bounds.extents.x, target.transform.position.x + target.GetComponent<Collider>().bounds.extents.x),
            target.transform.position.y + target.GetComponent<Collider>().bounds.extents.y,
            Random.Range(target.transform.position.z - target.GetComponent<Collider>().bounds.extents.z, target.transform.position.z + target.GetComponent<Collider>().bounds.extents.z)
            );
    }

    public void toggleCrossWalk(int direction, bool active)
    {
        foreach(GameObject corner in corners)
        {
            switch(direction)
            {
                case 1:
                    corner.transform.Find("roadA").gameObject.SetActive(active);
                    break;
                case 2:
                    corner.transform.Find("roadB").gameObject.SetActive(active);
                    break;
            }
            
        }
    }

    void Update()
    {
        if(getDistanceToPlayer() <= pool.distanceToSpawn && playerNearby == false) // get when vehicle gets close?
        {
            playerNearby = true;

            foreach(GameObject corner in corners)
            {
                int amountSpawn = Random.Range(0, maxSpawnPerCorner);
                minX = corner.transform.position.x - corner.GetComponent<Collider>().bounds.extents.x;
                maxX = corner.transform.position.x + corner.GetComponent<Collider>().bounds.extents.x;

                minZ = corner.transform.position.z - corner.GetComponent<Collider>().bounds.extents.z;
                maxZ = corner.transform.position.z + corner.GetComponent<Collider>().bounds.extents.z;

                

                for (int i = 0; i < amountSpawn; i++)
                {
                    pool.getPedestrian(new Vector3(Random.Range(minX, maxX), corner.transform.position.y + corner.GetComponent<Collider>().bounds.extents.y, Random.Range(minZ, maxZ))).GetComponent<pedestrianObject>().currentIntersection = this;
                }
            }
        }
        else if(getDistanceToPlayer() > pool.distanceToSpawn)
        {
            playerNearby = false;
        }
    }
}
