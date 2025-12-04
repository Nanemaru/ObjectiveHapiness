using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public string job;
    public Transform farm;
    public Transform forest;
    public Transform mine;
    public int age = 0;
    public NavMeshAgent agent;
    public float range; //radius of sphere
    public Transform centrePoint; //centre of the area the agent wants to move around in instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area
    public bool hunger = false;
    private bool _tired = false;
    public bool isOccupied = false;
    private bool _joy = true;
    private bool _home = false;
    private Transform _homePosition;
    private int _deathage;
    private Vector3 destinationWhenResume;
    private GameManager gameManager;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _deathage = Random.Range(8, 12);
        CheckAHomeAvailable();
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        if (!_tired && !isOccupied)
        {
            SetupAgent(job);
        }
        else
        {
            _joy = false;
            MakePnjWander();
        }
    }
    private void CheckAHomeAvailable()
    {
        foreach (var home in gameManager.homes)
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
        if (hunger || age == _deathage)
        {
            gameManager._numberPnjOnGame.Remove(gameObject.GetComponent<Character>());
            Destroy(gameObject);
        }
    }

    public void PnjTired() //Function when PnjTired
    {
        _tired = true;
        if (!_home) CheckAHomeAvailable();
        if (_home) agent.SetDestination(_homePosition.position);
        
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
