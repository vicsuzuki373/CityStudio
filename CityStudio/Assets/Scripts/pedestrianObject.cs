using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class pedestrianObject : MonoBehaviour
{
    pedestrians poolParent;
    [Header("Jaywalker")]
    public GameObject player;
    public GameObject overheadCam;
    public GameController gameController;

    [Header("Pedestrian")]
    public float maxWaitTime;
    private NavMeshPath path;
    private float elapsed = 0.0f;

    public Vector3 target;
    public float distanceToNewPath;
    public intersection currentIntersection;

    public Animator pedestrianAnimation;
    private Vector3 prevPosi;
    [Range(0.0f, 5.0f)]
    public float speedModifier = 0.5f; // this seems to be around the correct speed

    public Vector3 initialJayWalkerPosi;


    void Start()
    {
        if (currentIntersection != null)
        {
            poolParent = gameObject.transform.parent.GetComponent<pedestrians>();
            target = currentIntersection.sendPedestrianTarget();
            path = new NavMeshPath();
        }

        pedestrianAnimation.Play("Walking");
        pedestrianAnimation.speed = 0;
        prevPosi = transform.position;

        initialJayWalkerPosi = transform.position;
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

        if (currentIntersection != null) // if not a jaywalker (has navmeshagent active and uses the pedestrian pool)
        {
            distanceToTarget();
            recalculatePath(Random.Range(0.0f, maxWaitTime));

            if (currentIntersection.playerNearby == false)
            {
                currentIntersection = null;
                poolParent.stashPedestrian(this.gameObject); // stash the pedestrian if gets too far?
            }
        }
        else
        {
            actAsJayWalker();
        }
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

    void actAsJayWalker()
    {
        if(Vector3.Distance(gameObject.transform.position, player.transform.position) < 5 && !overheadCam.activeInHierarchy) // this seems the right distance
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime);


    }

    public void restart()
    {
        StartCoroutine("resetJayWalker", 0.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && currentIntersection != null)
        {
            currentIntersection = null;
            poolParent.stashPedestrian(this.gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            StartCoroutine("resetJayWalker", 30);
            gameObject.transform.position = new Vector3(0, 1000000, 0);     //someplace high up when they die cause coroutine dont work when they get set inactive
        }
    }

    private IEnumerator resetJayWalker(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        gameObject.transform.position = initialJayWalkerPosi;
    }
}
