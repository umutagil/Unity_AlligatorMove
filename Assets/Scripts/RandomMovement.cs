using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

[RequireComponent (typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class RandomMovement : MonoBehaviour
{

    [Range(0, 1)]
    public float randomness = 0f;           //decides how random the agent moves
    public float speed = 1f;                //speed of agent
    public float timeBeforeDecide = 1f;     //timestep duration of decided action
    public Transform boundaries;            //parent object of corner point gameobjects

    private NavMeshAgent agent;
    private Animator anim;        
    private Borders borders;                //object that holds border corners and border lines
    private Vector3 nextDestination;        //next destination of the agent
    private Vector3 prevDestination;        //last successfull destination of the agent
    private Vector3 startPos;               //starting position of the agent    
    private int destinationTryCount = 5;    //how many times should the agent try to find a destination that is inside borders
    private float lastChangeTime = 0f;

    void OnValidate()
    {
        Assert.IsNotNull(boundaries, "Boundaries not assigned!");
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        Assert.IsNotNull(boundaries, "Boundaries not assigned!");        

        startPos = transform.position;
        prevDestination = transform.position;
        nextDestination = transform.position + transform.forward;
        agent.SetDestination(nextDestination);
        agent.speed = speed;        
        borders = ComputeBorders(boundaries);        
    }

    void Update()
    {
        Assert.IsFalse((speed < 0), "Speed is lower than zero!");
        Assert.IsFalse((timeBeforeDecide < 0), "timeBeforeDecide is lower than zero!");

        ComputeNextDestination();
        agent.speed = speed;
        agent.stoppingDistance = speed * 0.2f;
        SetAnimParameters();        
    }

    //Sets speed parameter of the animator controller
    void SetAnimParameters()
    {
        float maxSpeed = speed > 0 ? speed : 1;
        anim.SetFloat("speed", Vector3.Magnitude(agent.velocity) / maxSpeed);
    }
    
    //At the end of every timestep randomly decide the next destination
    void ComputeNextDestination()
    {
        if (Time.time - lastChangeTime > timeBeforeDecide)
        {
            lastChangeTime = Time.time;
            if (Random.value > 1.0f - randomness)
            {
                float rotAngle = (Random.value * 2 - 1f)  * 90;
                Vector3 nextDirection = Quaternion.AngleAxis(rotAngle, Vector3.up) * transform.forward;
                nextDestination = transform.position + nextDirection * speed;
            }
            else
            {
                nextDestination = transform.position + transform.forward * speed;
            }

            if(borders.IsDestinationOut(prevDestination, nextDestination))
            {
                nextDestination = FindRandomDestionationInside();
            }
            
            prevDestination = agent.destination;
            agent.SetDestination(nextDestination);
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            nextDestination = transform.position + transform.forward * speed;
            if (borders.IsDestinationOut(prevDestination, nextDestination))
            {
                nextDestination = FindRandomDestionationInside();
            }            

            prevDestination = agent.destination;
            agent.SetDestination(nextDestination);
        }

        Debug.DrawLine(transform.position, nextDestination, Color.red);
    }

    //This is called if the agents reaches borders
    //Tries to find a random destination inside the borders
    Vector3 FindRandomDestionationInside()
    {
        Vector3 destination = startPos;
        for (int i = 0; i < destinationTryCount; i++)
        {
            float rotAngle = (Random.value * 2 - 1f) * 90;
            Vector3 nextDirection = Quaternion.AngleAxis(rotAngle, Vector3.up) * -transform.forward;
            destination = transform.position + nextDirection * speed;

            if (!borders.IsDestinationOut(prevDestination, destination))
            {
                break;
            }

            if (i == (destinationTryCount - 1))
            {
                destination = startPos;
            }                
        }

        return destination;
    }

    //gets positions of border corners objects
    Borders ComputeBorders(Transform bordersObj)
    {
        List<Vector3> cornerPosList = new List<Vector3>();
        for (int i = 0; i < bordersObj.childCount; i++)
        {
            Vector3 cornerPos = bordersObj.GetChild(i).position;
            cornerPosList.Add(cornerPos);
        }

        return new Borders(cornerPosList);
    }

}