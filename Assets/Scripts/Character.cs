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
    public HomeClass _homePosition;
    private int _ageOfDeath;
    public int resourcesToGive = 0;

    public string newJob;

    [SerializeField] private GameObject isWorkingAsset;
    //Character stat
    private bool _tired = false;
    private bool _home = false;
    public bool goingToSchool = false;
    private bool isLearning = false;
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
                _homePosition = home;
                if (home.NumberBedLeft == 0) home.IsAvailable = false;
                break;
            }
        }
    }

    private void SetADestination(GameObject[] objects)
    {
        int arrayIndex = Random.Range(0, objects.Length);
        agent.SetDestination(objects[arrayIndex].transform.position);
    }

    private void CheckIfPnjStillAlive()
    {
        UpdateResources();
        age++;
        if (age >= _ageOfDeath)
        {
            if (_home) _homePosition.NumberBedLeft++;
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
            agent.SetDestination(_homePosition.transform.position);
            _tired = false;
        }
        else //If pnj doesn't find a home they wander until it find a home available
        {
            prosperityToAdd = -3f;
            MakePnjWander();
            _gameManager.UpdateProsperity(prosperityToAdd);
        }
    }

    private void SetupAgent() //Give A destination to pnj based on their job
    {
        switch (job)
        {
            case "farmer":
                SetADestination(GameObject.FindGameObjectsWithTag("farmer"));
                break;
            case "lumberjack":
                SetADestination(GameObject.FindGameObjectsWithTag("lumberjack"));
                break;
            case "miner":
                SetADestination(GameObject.FindGameObjectsWithTag("miner"));
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

    private void OnTriggerEnter(Collider other) //Check if pnj are in their workzone or home or school
    {
        switch (job)
        {
            case "wanderer":
                break;
            case "farmer" when Mathf.Approximately(agent.destination.x, other.gameObject.transform.position.x) && Mathf.Approximately(agent.destination.z, other.gameObject.transform.position.z):
                resourcesToGive += 2 * _gameManager.foodMultiplicator;
                WorkingAsset(true);
                break;
            default:
            {
                if (Mathf.Approximately(agent.destination.x, other.gameObject.transform.position.x) && Mathf.Approximately(agent.destination.z, other.gameObject.transform.position.z))
                {
                    resourcesToGive += 3;
                    WorkingAsset(true);
                }
                break;
            }
        }
        if (job != "wanderer" && _home && Mathf.Approximately(other.transform.position.x, _homePosition.transform.position.x) && Mathf.Approximately(other.transform.position.z, _homePosition.transform.position.z))
        {
            StartCoroutine(PnjSleeping());
        }
        else if (other.transform == _gameManager.school && !isLearning && goingToSchool)
        {
            isLearning = true;
            goingToSchool = false;
            StartCoroutine(PnjLearning());
        }
    }

    private void WorkingAsset(bool enabledState)
    {
        isWorkingAsset.SetActive(enabled);
        isWorkingAsset.transform.LookAt(_gameManager.tranformOfWorkinAssetTarget);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == _gameManager.school && isLearning) isLearning = false;
        if (isWorkingAsset.activeSelf) WorkingAsset(false);
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
        GameObject OldAppearance = transform.GetChild(0).gameObject;
        Destroy(OldAppearance);
        GameObject newAppearance = Instantiate(jobPrefab[Random.Range(0, jobPrefab.Length)], transform, true);
        newAppearance.transform.position = transform.position;
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
        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager.NewPopUp(uiManager.deadPnj);
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
