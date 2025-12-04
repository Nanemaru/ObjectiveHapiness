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
    private NavMeshAgent _agent;
    public float range; //radius of sphere
    public Transform centrePoint; //centre of the area the agent wants to move around in instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area
    private bool _hunger = false;
    private bool _tired = false;
    private bool _isOccupied = false;
    private bool _joy = true;
    private bool _home = false;
    private Transform _homePosition;
    private int _deathage;
    
    private GameManager gameManager;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _deathage = Random.Range(8, 12);
        CheckAHomeAvailable();
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        if (!_tired && !_isOccupied)
        {
            switch (job)
            {
                case "farmer":
                    SetADestination(farm);
                    break;
                case "lumberjack":
                    SetADestination(forest);
                    break;
                case "miner":
                    SetADestination(mine);
                    break;
                case "mason":
                    if (_agent.remainingDistance <= _agent.stoppingDistance) //done with path
                    {
                        
                        if (construct is not null)
                        {
                            _agent.SetDestination(construct.position);
                        }
                        Vector3 point;
                        if(RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
                        {
                            _agent.SetDestination(point);
                        }
                        
                    }
                    break;
                case "wanderer":
                    if (_agent.remainingDistance <= _agent.stoppingDistance)
                    {
                        Vector3 point;
                        if (RandomPoint(centrePoint.position, range, out point))
                        {
                            _agent.SetDestination(point);
                        }
                    }
                    
                    break;
            }
        }
        else
        {
            _joy = false;
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, range, out point))
                {
                    _agent.SetDestination(point);
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
    
    private void CheckAHomeAvailable()
    {
        for (int i = 0; i < gameManager.homes.Count;i++)
        {
            HomeClass actualHome = gameManager.homes[i].GetComponent<HomeClass>();
            if (actualHome.IsAvailable)
            {
                actualHome.IsAvailable = false;
                _home = true;
                _homePosition = gameManager.homes[i].transform;
                break;
            }
        }
    }

    private void SetADestination(Transform destination)
    {
        _agent.SetDestination(destination.position);
        _isOccupied = true;
    }

    public void CheckIfPnjStillAlive()
    {
        if (_hunger || age == _deathage)
        {
            gameManager._numberPnjOnGame.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void PnjTired() //Function when PnjTired
    {
        _tired = true;
        if (!_home)
        {
            CheckAHomeAvailable();
        }
        if (_home) _agent.SetDestination(_homePosition.position);
        
    }
}
