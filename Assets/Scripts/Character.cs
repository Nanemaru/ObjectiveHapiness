using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
    private GameManager _gameManager;
    //Character Data
    public string job = "wanderer";
    public int age = 0;
    public NavMeshAgent agent;
    private Transform _homePosition;
    private int _ageOfDeath;
    public int resourcesToGive = 0;
    //Character state
    public bool hunger = false;
    public bool _tired = false;
    public bool _home = false;
    //Value for mouvement
    private Vector3 _destinationWhenResume;
    public float range;
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        _ageOfDeath = Random.Range(8, 12);
        CheckAHomeAvailable();
        SetupAgent();
    }
    void Update()
    {
        if (job == "wanderer" || _tired)
        {
            if (agent.remainingDistance <= agent.stoppingDistance) //Make pnj wander as long as they're tired ou wanderer
            {
                MakePnjWander();
            }
        }
    }
    private void CheckAHomeAvailable()
    {   
        if (job == "wanderer") return;
        foreach (var home in _gameManager.homes)
        {
            if (home.IsAvailable)
            {
                home.IsAvailable = false;
                _home = true;
                _homePosition = home.transform;
                break;
            }
        }
    }

    private void SetADestination(Transform destination)
    {
        agent.SetDestination(destination.position);
    }

    public void CheckIfPnjStillAlive()
    {
        if (hunger || age == _ageOfDeath)
        {
            _gameManager._numberPnjOnGame.Remove(this);
            Destroy(gameObject);
        }
    }

    public void PnjTired() //Function when Pnj is Tired
    {
        if (job == "wanderer") return;
        else _tired = true;
        float prosperityToAdd;
        if (!_home) CheckAHomeAvailable();
        if (_home)
        {
            agent.SetDestination(_homePosition.position);
            prosperityToAdd = 2f;
            _tired = false;
        }
        else //If pnj doesn't find a home it wander until it find a home available
        {
            prosperityToAdd = -3f;
            MakePnjWander();
        }
        _gameManager.UpdateProsperity(prosperityToAdd);
        
    }

    private void SetupAgent() //Give A destination to pnj based on their job
    {
        switch (job)
        {
            case "farmer":
                SetADestination(_gameManager.farm);
                break;
            case "lumberjack":
                SetADestination(_gameManager.forest);
                break;
            case "miner":
                SetADestination(_gameManager.mine);
                break;
            case "mason":
            case "wanderer":
                MakePnjWander();
                break;
        }
    }

    private void MakePnjWander()
    {
        bool destinationOnMesh = false;
        Vector3 destination = new Vector3();
        while (!destinationOnMesh)
        {
            Vector3 randomPoint = _gameManager.centrePoint.position + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                destination = hit.position;
                destinationOnMesh = true;
            }
        }
        agent.SetDestination(destination);
    }

    private void OnTriggerEnter(Collider other) //Check if pnj are in their workzone or home
    {
        if (other.CompareTag(job))
        {
            resourcesToGive += 3 * _gameManager.foodMultiplicator;
        }
        
        else if (other.transform == _homePosition)
        {
            _tired = false;
            StartCoroutine(PnjSleeping());
            
        }
    }
    
    IEnumerator PnjSleeping() //Coroutine to let Pnj sleep before return to work
    {
        yield return new WaitForSeconds(5);
        SetupAgent();
    }
}
