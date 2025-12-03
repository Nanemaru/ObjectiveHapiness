using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public string job;
    public Transform farm;
    public Transform forest;
    public Transform mine;
    public Transform construct = null;
    public int age = 0;
    private NavMeshAgent agent;
    public float range; //radius of sphere
    public Transform centrePoint; //centre of the area the agent wants to move around in instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area
    private bool Hunger = false;
    private bool Tired = false;
    private bool Joy = true;
    private int deathage;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        deathage = Random.Range(8, 12);

    }
    void Update()
    {
        if (Hunger == true || age == deathage)
        {
                Destroy(gameObject);
        }
        if (Tired == false)
        {
            switch (job)
            {
                case "farmer":
                    agent.SetDestination(farm.position);
                    break;
                case "lumberjack":
                    agent.SetDestination(forest.position);
                    break;
                case "miner":
                    agent.SetDestination(mine.position);
                    break;
                case "mason":
                    if (agent.remainingDistance <= agent.stoppingDistance) //done with path
                    {
                        Vector3 point;
                        if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
                        {
                            agent.SetDestination(point);
                        }
                    }
                    break;
                case "wanderer":
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        Vector3 point;
                        if (RandomPoint(centrePoint.position, range, out point))
                        {
                            agent.SetDestination(point);
                        }
                    }
                    if (construct != null)
                    {
                        agent.SetDestination(construct.position);
                    }
                    break;
            }
        }
        if (Tired == true)
        {
            Joy = false;
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, range, out point))
                {
                    agent.SetDestination(point);
                }
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    int growOld(age)
    {
        return age++;
    }
}
