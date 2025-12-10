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

    public string newJob;
    //Character state
    public bool hunger = false;
    private bool _tired = false;
    private bool _home = false;
    public bool goingToSchool = false;
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
        if (agent.remainingDistance <= agent.stoppingDistance && !goingToSchool) //Make pnj wander as long as they're tired, wanderer, and not going to school
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
                if (home.NumberBedLeft == 0) home.IsAvailable = false;
                break;
            }
        }
    }

    public void SetADestination(Transform destination)
    {
        agent.SetDestination(destination.position);
    }

    private void CheckIfPnjStillAlive()
    {
        UpdateResources();
        age++;
        if (age == _ageOfDeath)
        {
            if (_home) gameObject.GetComponent<HomeClass>().NumberBedLeft++;
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
        else //If pnj doesn't find a home they wander until it find a home available
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
        switch (job)
        {
            case "wanderer":
                break;
            case "farmer" when other.CompareTag(job):
                resourcesToGive += 3 * _gameManager.foodMultiplicator;
                break;
            default:
            {
                if (other.CompareTag(job)) resourcesToGive += 3;
                break;
            }
        }
        if (job != "wanderer" && other.transform == _homePosition) StartCoroutine(PnjSleeping());
        else if (other.transform == _gameManager.school) StartCoroutine(PnjLearning());
    }

    private void ChangePnjAppearance()
    {
        switch (job)
        {
            case "farmer":
                GiveTheNewAppearance(_gameManager.farmerPrefab);
                break;
            case "lumberjack":
                GiveTheNewAppearance(_gameManager.lumberjackPrefab);
                break;
            case "miner":
                GiveTheNewAppearance(_gameManager.minerPrefab);
                break;
            case "mason":
                GiveTheNewAppearance(_gameManager.masonPrefab);
                break;
        }
    }

    private void GiveTheNewAppearance(GameObject[] jobPrefab)
    {
        GameObject OldAppearance = this.transform.GetChild(0).gameObject;
        Quaternion Rotation = OldAppearance.transform.rotation;
        Rotation.y += 180;
        Destroy(OldAppearance);
        GameObject newAppearance = Instantiate(jobPrefab[Random.Range(0, jobPrefab.Length)], this.transform, true);
        newAppearance.transform.position = this.transform.position;
        newAppearance.transform.rotation = Rotation;
    }
    

    private void FeedPnj()
    {
        switch (_gameManager._numberFood)
        {
            case >= 1:
                _gameManager._numberFood--;
                break;
            case 0:
                KillPnj();
                break;
        }
    }

    private void KillPnj()
    {
        _gameManager.numberOfPnj--;
        if (_gameManager.numberOfPnj <= 0) _gameManager.LoseGame();
        if (job == "mason") _gameManager._numberMason--;
        Destroy(gameObject);
    }
    
    IEnumerator PnjSleeping() //Coroutine to let Pnj sleep before return to work, also use when pnj is in school for professional retraining
    {
        yield return new WaitForSeconds(5);
        _tired = false;
        SetupAgent();
    }
    IEnumerator PnjLearning() //Coroutine to let pnj being in school for professional retraining
    {
        yield return new WaitForSeconds(5);
        job = newJob;
        newJob = String.Empty;
        ChangePnjAppearance();
        if (job == "mason") _gameManager._numberMason++;
        goingToSchool = false;
        SetupAgent();
    }
    
    private void PutPnjInResume()
    {
        agent.isStopped = !_gameManager.IsOnPlay;
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
