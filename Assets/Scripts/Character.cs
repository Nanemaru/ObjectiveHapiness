using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public string job = "wanderer";
    public Transform farm;
    public Transform forest;
    public Transform mine;
    public int age = 0;
    public NavMeshAgent agent;
    public float range; //radius of sphere
    public Transform centrePoint; //centre of the area the agent wants to move around in instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area
    public bool hunger = false;
    public bool _tired = false;
    public bool isOccupied = false;
    public bool _home = false;
    private Transform _homePosition;
    private int _ageOfDeath;
    private Vector3 _destinationWhenResume;
    private GameManager _gameManager;
    public int resourcesToGive = 0;
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        _ageOfDeath = Random.Range(8, 12);
        CheckAHomeAvailable();
        SetupAgent(job);
    }
    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.isStopped)
        {
            if (!isOccupied) MakePnjWander();
            else
            {
                //Faire une animation ?
                resourcesToGive += 3 * _gameManager.foodMultiplicator;
            }
        }

        /*if (_tired)
        {
            PnjTired(); 
        }*/
    }
    private void CheckAHomeAvailable()
    {   
        if (job == "wanderer") return;
        foreach (var home in _gameManager.homes)
        {
            HomeClass actualHome = home.GetComponent<HomeClass>();
            if (actualHome.IsAvailable)
            {
                actualHome.IsAvailable = false;
                _home = true;
                _homePosition = home.transform;
                break;
            }
        }
    }

    private void SetADestination(Transform destination)
    {
        agent.SetDestination(destination.position);
        isOccupied = true;
    }

    public void CheckIfPnjStillAlive()
    {
        if (hunger || age == _ageOfDeath)
        {
            _gameManager._numberPnjOnGame.Remove(gameObject.GetComponent<Character>());
            Destroy(gameObject);
        }
    }

    public void PnjTired() //Function when PnjTired
    {
        if (job != "wanderer") _tired = true;
        float prosperityToAdd;
        if (!_home) CheckAHomeAvailable();
        if (_home)
        {
            agent.SetDestination(_homePosition.position);
            prosperityToAdd = 2f;
            _tired = false;
        }
        else
        {
            prosperityToAdd = -0.1f;
            MakePnjWander();
        }
        _gameManager.UpdateProsperity(prosperityToAdd);
        
    }

    private void SetupAgent(string job) //Give A destination to pnj based on their job
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
            Vector3 randomPoint = centrePoint.position + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                destination = hit.position;
                destinationOnMesh = true;
            }
        }
        agent.SetDestination(destination);
    }

}
