using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
    private GameManager _gameManager;

    private UnityEvent eventAge;
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
        agent = GetComponent<NavMeshAgent>();
        _ageOfDeath = Random.Range(8, 12);
        _gameManager = FindObjectOfType<GameManager>();
        _gameManager._eventUpdatePnj.AddListener(CheckIfPnjStillAlive);
        _gameManager._eventPnjInResume.AddListener(PutPnjInResume);
        CheckAHomeAvailable();
        SetupAgent();
    }
    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) //Make pnj wander as long as they're tired ou wanderer
        {
            if (job == "wanderer") MakePnjWander();
            else if (_tired) CheckAHomeAvailable();
        }
    }
    private void CheckAHomeAvailable()
    {   
        if (job == "wanderer") return;
        foreach (var home in _gameManager.homes)
        {
            if (home.IsAvailable)
            {
                home.NumberBedLeft--;
                _home = true;
                _homePosition = home.transform;
                break;
            }

            if (home.NumberBedLeft <= 0)
            {
                home.NumberBedLeft = 0;
                home.IsAvailable = false;
            }
        }
    }

    private void SetADestination(Transform destination)
    {
        agent.SetDestination(destination.position);
    }

    private void CheckIfPnjStillAlive()
    {
        UpdateResources();
        age++;
        if (age == _ageOfDeath)
        {
            gameObject.GetComponent<HomeClass>().NumberBedLeft++;
            KillPnj();
        }
        FeedPnj();
        PnjTired();
    }

    private void PnjTired() //Function when Pnj is Tired
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
                SetADestination(_gameManager.bush);
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
        if (job != "wanderer" && other.CompareTag(job))
        {
            resourcesToGive += 3 * _gameManager.foodMultiplicator;
        }
        
        else if (other.transform == _homePosition)
        {
            _tired = false;
            StartCoroutine(PnjSleeping());
            
        }
    }

    private void FeedPnj()
    {
        if (_gameManager._numberFood > 1) _gameManager._numberFood--;
        else if (_gameManager._numberFood == 0) KillPnj();
    }

    private void KillPnj()
    {
        _gameManager.numberOfPnj--;
        Destroy(gameObject);
        if (_gameManager.numberOfPnj <= 0) _gameManager.LoseGame();
    }
    
    IEnumerator PnjSleeping() //Coroutine to let Pnj sleep before return to work
    {
        yield return new WaitForSeconds(5);
        SetupAgent();
    }
    
    private void PutPnjInResume()
    {
        agent.isStopped = _gameManager.IsOnPlay;
    }
    
    private void UpdateResources()
    {
        switch (job)
        {
            case "farmer":
                _gameManager._numberFood += resourcesToGive;
                break;
            case "lumberjack":
                _gameManager._numberWood += resourcesToGive;
                break;
            case "miner":
                _gameManager._numberStone  += resourcesToGive;
                break;
        }
        resourcesToGive = 0;
    }
}
