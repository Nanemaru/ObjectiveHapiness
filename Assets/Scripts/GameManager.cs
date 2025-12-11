using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    //Time management
    private float _dayDuration = 60f; //Set the duration in seconds of a day 
    private float _numberSecondOfDay = 0f;
    public int numberDay = 1;
    private bool _isOnPlay = true; //Value to use to put game in resume
    private float _numberSecondForABirth = 60f;
    private float _SecondForBirthCounter;
    public bool IsOnPlay
    {
        get => _isOnPlay;
        private set => _isOnPlay = value;
    }
    
    //Define how much resource there is in game
    public int _numberFood = 20; 
    public int _numberWood = 0;
    public int _numberStone = 0;
    public int foodMultiplicator = 1;
    public float prosperity = 5f;
        
    //PNJ
    [SerializeField] private List<GameObject> typeOfPnj = new List<GameObject>(); //script for prefab pnj
    [SerializeField] private GameObject wanderer;
    public int numberOfPnj = 4;
    public int _numberMason = 1;
    
    public GameObject[] farmerPrefab =  new GameObject[2];
    public GameObject[] lumberjackPrefab =  new GameObject[2];
    public GameObject[] minerPrefab =  new GameObject[2];
    public GameObject[] masonPrefab =  new GameObject[2];
    
    private readonly Vector3 _positionSpawn = new Vector3(459, 0, -152);
    //Event
    public UnityEvent _eventUpdatePnj;
    public UnityEvent _eventPnjInResume;
    
    //Buildings
    public List<HomeClass> homes = new List<HomeClass>();

    public Transform school;
    public bool _isSchoolCreate = false;
    
    //Work Zone
    public Transform bush;
    public Transform forest;
    public Transform mine;
    public Transform centrePoint;

    //UI Endgame
    public GameObject win;
    public GameObject lose;
    
    [SerializeField] private UIManager uiManager;
    /*void Start()
    {
       UpdateDayCounter(); //Put text in unity directly so it doesn't need to be call at start
    }*/

    void Update()
    {
        if (_isOnPlay)
        {
            UpdateTimeAndDay();
        }
    }

    private void WinGame() //Function call when prosperity reach 100%
    {
        win.SetActive(true);
    }
    
    public  void LoseGame() //Function call when all pnj are dead
    {
        lose.SetActive(true);
    }

    private void NextDay() //Function call to pass to next day
    {
        _numberSecondOfDay = 0f;
        numberDay++;
        _eventUpdatePnj.Invoke();
        uiManager.UpdateResourceText();
        uiManager.UpdateDayCounter();
    }
    public void UpdateProsperity(float value) //Function call to update prosperity with positive or negative value
    {
        prosperity += value;
        if (prosperity < 0) prosperity = 0f;
        uiManager.UpdateSliderProsperity(prosperity);
        if (prosperity >= 100) WinGame();
    }

    public void Resume() //Put game from play to resume and vice versa
    {
        IsOnPlay = !IsOnPlay;
        _eventPnjInResume.Invoke();
    }

    private void UpdateTimeAndDay()
    {
        _numberSecondOfDay  += Time.deltaTime;
        _SecondForBirthCounter += Time.deltaTime;
        if  (_SecondForBirthCounter >= _numberSecondForABirth) //Create a wanderer each X seconds
        {
            Instantiate(wanderer, _positionSpawn, Quaternion.identity);
            numberOfPnj++;
            _SecondForBirthCounter = 0f;
        }
        if (_numberSecondOfDay > _dayDuration) NextDay();
    }
    
}
