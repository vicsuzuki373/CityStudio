using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class pedestrianObject : MonoBehaviour
{
    pedestrians poolParent;

    private NavMeshPath path;
    private float elapsed = 0.0f;
    public float maxWaitTime;

    public Vector3 target;
    public float distanceToNewPath;
    public intersection currentIntersection; // change to private after testing

    public Animator pedestrianAnimation;
    private Vector3 prevPosi;
    [Range(0.0f, 5.0f)]
    public float speedModifier = 0.5f; // this seems to be around the correct speed


    void Start()
    {
        poolParent = gameObject.transform.parent.GetComponent<pedestrians>();
        target = currentIntersection.sendPedestrianTarget();
        path = new NavMeshPath();

        pedestrianAnimation.Play("Walking");
        pedestrianAnimation.speed = 0;
        prevPosi = transform.position;
    }

    void animate()
    {
        Vector3 currentMove = gameObject.transform.position - prevPosi;
        pedestrianAnimation.speed = (currentMove.magnitude / Time.deltaTime) * speedModifier;
        prevPosi = gameObject.transform.position;


        if (pedestrianAnimation.speed <= 0.05f)
        {
            pedestrianAnimation.Play("Idle");
        }
        else
            pedestrianAnimation.Play("Walking");
    }

    void Update()
    {
        animate();
        distanceToTarget();
        recalculatePath(Random.Range(0.0f, maxWaitTime));

        if (currentIntersection.playerNearby == false)
            poolParent.stashPedestrian(this.gameObject); // stash the pedestrian if gets too far?
    }

    void distanceToTarget()
    {
        if(Vector3.Distance(gameObject.transform.position, target) < distanceToNewPath)
        {
            target = currentIntersection.sendPedestrianTarget();
            recalculatePath(Random.Range(0.0f, maxWaitTime));
        }
    }

    void recalculatePath(float wait)
    {
        elapsed += Time.deltaTime;
        if (elapsed > wait)
        {
            elapsed = 0.0f;

            try
            { this.gameObject.GetComponent<NavMeshAgent>().SetDestination(target); }
            catch { }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            poolParent.stashPedestrian(this.gameObject);
        }
    }

}
