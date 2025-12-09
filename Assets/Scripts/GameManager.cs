using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    //Time management
    private float _dayDuration = 20f; //Set the duration in seconds of a day 
    private float _numberSecondOfDay = 0f;
    public int numberDay = 1;
    private bool _isOnPlay = true; //Value to use to put game in resume
    private float _numberSecondForABirth = 30f;
    private float _SecondForBirthCounter;
    public bool IsOnPlay
    {
        get => _isOnPlay;
        set => _isOnPlay = value;
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
    //public List<Character> _numberPnjOnGame = new List<Character>();
    public List<GameObject> _numberMason = new List<GameObject>();
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
        
    }
    
    private  void LoseGame() //Function call when all pnj are dead
    {
        Debug.Log("LoseGame");
    }

    private void NextDay() //Function call to pass to next day
    {
        _numberSecondOfDay = 0f;
        numberDay++;
        UpdatePnj();
        uiManager.UpdateResourceText();
        uiManager.UpdateDayCounter();
        //if (!FindAnyObjectByType<Character>()) LoseGame();
        Debug.Log(FindAnyObjectByType<Character>());
    }

    private void UpdatePnj() //Function call to update pnj
    {
        _eventUpdatePnj.Invoke();
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
        _isOnPlay = !_isOnPlay;
        //Change sprite
        _eventPnjInResume.Invoke();
    }

    private void UpdateTimeAndDay()
    {
        _numberSecondOfDay  += Time.deltaTime;
        _SecondForBirthCounter += Time.deltaTime;
        if  (_SecondForBirthCounter >= _numberSecondForABirth) //Create a wanderer each X seconds
        {
            Instantiate(wanderer);
            _SecondForBirthCounter = 0f;
        }
        if (_numberSecondOfDay > _dayDuration) NextDay();
    }
    
}
