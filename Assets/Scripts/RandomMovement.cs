using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour {

    public float speed = 5f;
    public float debugRotAngle = 0f;
    public Transform boundaries;

    [Range(0,1)]
    public float randomness = 0f;

    private NavMeshAgent agent;
    private Animator anim;
    private float lastChangeTime = 0f;
    private float timeToChange = 1f;
    private Vector3 nextDestination;
    private Borders borders;
    private List<Vector3> boundaryCorners;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

    }

	void Start () {
        nextDestination = transform.position + transform.forward;
        agent.SetDestination(nextDestination);
        agent.speed = speed;
        borders = ComputeBorders(boundaries);
        boundaryCorners = GetBoundaryCorners(boundaries);
    }
    
    void Update () {

        agent.speed = speed;
        //anim.SetFloat("speed", speed);

        if (Time.time - lastChangeTime > timeToChange)
        {
            lastChangeTime = Time.time;
            if(Random.value > 1.0f - randomness)
            {
                float rotAngle = (Random.value - 0.5f) * 90;                
                Vector3 nextDirection = Quaternion.AngleAxis(rotAngle, Vector3.up) * transform.forward;
                nextDestination = transform.position + nextDirection;
            }
            else
            {
                nextDestination = transform.position + transform.forward;                
            }

            agent.SetDestination(nextDestination);
        }
        else if(agent.remainingDistance <= agent.stoppingDistance)
        {
            nextDestination = transform.position + transform.forward;
            agent.SetDestination(nextDestination);
        }

	}

    bool isNewDestionationInBoundary(List<Vector3> boundaryCorners)
    {
        return false;
    }

    List<Vector3> GetBoundaryCorners(Transform boundaryTransform)
    {
        List<Vector3> cornerPosList = new List<Vector3>();
        for (int i = 0; i < boundaryTransform.childCount; i++)
        {
            Vector3 cornerPos = boundaryTransform.GetChild(i).position;
            cornerPosList.Add(cornerPos);
        }
        return cornerPosList;
    }

    Borders ComputeBorders(Transform bordersObj)
    {
        List<Vector3> cornerPosList = new List<Vector3>();
        for(int i = 0; i < bordersObj.childCount; i++)
        {
            Vector3 cornerPos = bordersObj.GetChild(i).position;
            cornerPosList.Add(cornerPos);
        }

        return new Borders(cornerPosList);
    }

}
