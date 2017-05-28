using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{

    [Range(0, 1)]
    public float randomness = 0f;
    public float speed = 5f;
    public float timeBeforeDecide = 1f;
    public Transform boundaries;    

    private NavMeshAgent agent;
    private Animator anim;    
    private Vector3 nextDestination;
    private Borders borders;
    private List<Vector3> boundaryCorners;
    private float lastChangeTime = 0f;
    private Vector3 prevDestination;
    private Vector3 startPos;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        startPos = transform.position;
        prevDestination = transform.position;
        nextDestination = transform.position + transform.forward;
        agent.SetDestination(nextDestination);
        agent.speed = speed;
        borders = ComputeBorders(boundaries);        
    }

    void Update()
    {        
        ComputeNextDestination();
        agent.speed = speed;
        agent.stoppingDistance = speed * 0.2f;
        float maxSpeed = speed > 0 ? speed : 1;
        anim.SetFloat("speed", Vector3.Magnitude(agent.velocity) / maxSpeed);        
    }

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

            for (int i = 0; i < 5; i++)
            {
                if (!IsNewDestionationOut(borders, prevDestination, nextDestination))
                {
                    break;
                }

                float rotAngle = (Random.value * 2 - 1f) * 90;
                Vector3 nextDirection = Quaternion.AngleAxis(rotAngle, Vector3.up) * -transform.forward;
                nextDestination = transform.position + nextDirection * speed;

                if (i == 4)
                    nextDestination = startPos;
            }
            prevDestination = agent.destination;
            agent.SetDestination(nextDestination);
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            nextDestination = transform.position + transform.forward * speed;

            for(int i = 0; i < 5; i++)
            {
                if (!IsNewDestionationOut(borders, prevDestination, nextDestination))
                {
                    break;
                }

                float rotAngle = (Random.value * 2 - 1f) * 90;
                Vector3 nextDirection = Quaternion.AngleAxis(rotAngle, Vector3.up) * -transform.forward;
                nextDestination = transform.position + nextDirection * speed;

                if (i == 4)
                    nextDestination = startPos;
            }
            prevDestination = agent.destination;
            agent.SetDestination(nextDestination);
        }

        Debug.DrawLine(transform.position, nextDestination, Color.red);
    }

    //check all the border lines if the new destination is out
    bool IsNewDestionationOut(Borders borders, Vector3 currentPos, Vector3 nextDestPos)
    {            
        Vector2 currentPosXZ = new Vector2(currentPos.x, currentPos.z);
        Vector2 nextDestPosXZ = new Vector2(nextDestPos.x, nextDestPos.z);

        for (int i = 0; i < borders.lineIndices.Count; i++)
        {
            int p1Index = borders.lineIndices[i].pos1;
            int p2Index = borders.lineIndices[i].pos2;
            Vector3 p1 = borders.cornerPosList[p1Index];
            Vector3 p2 = borders.cornerPosList[p2Index];
            Vector2 p1XZ = new Vector2(p1.x, p1.z);
            Vector2 p2XZ = new Vector2(p2.x, p2.z);

            bool intersects = GeometryExtensions.FastLineSegmentIntersection(p1XZ, p2XZ, currentPosXZ, nextDestPosXZ);
            if (intersects)
                return true;
        }

        return false;
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