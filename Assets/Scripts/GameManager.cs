using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    //Time management
    public float _prosperity = 5f;
    private float _dayDuration = 20f; //Set the duration in seconds of a day 
    private float _numberSecondOfDay = 0f;
    private int _numberDay = 1;
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
    //Canvas management
        [SerializeField] private TextMeshProUGUI dayCounter;
        
    //PNJ
    [SerializeField] private List<GameObject> typeOfPnj = new List<GameObject>(); //script for prefab pnj
    [SerializeField] private GameObject wanderer;
    public List<Character> _numberPnjOnGame = new List<Character>();
    public List<GameObject> _numberMason = new List<GameObject>();

    //Buildings
    public List<HomeClass> homes = new List<HomeClass>();

    public Transform school;
    public bool _isSchoolCreate = false;
    
    //Work Zone
    public Transform farm;
    public Transform forest;
    public Transform mine;
    public Transform centrePoint;
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
    }

    private void NextDay() //Function call to pass to next day
    {
        _numberSecondOfDay = 0f;
        _numberDay++;
        UpdateResources();
        NourrishPnj();
        UpdatePnj();
        UpdateDayCounter();
    }

    private void UpdatePnj() //Function call to update pnj
    {
        foreach (var pnj in _numberPnjOnGame)
        {
            pnj.age++;
            pnj.CheckIfPnjStillAlive();
            pnj.PnjTired();
        }
    }
    

    public void UpdateProsperity(float value) //Function call to update prosperity with positive or negative value
    {
        _prosperity += value;
        if (_prosperity < 0) _prosperity = 0f;
    }

    public void Resume() //Put game from play to resume and vice versa
    {
        _isOnPlay = !_isOnPlay;
        //Change sprite
        PutPnjOnResume();
    }

    public void PutPnjOnResume()
    {
        foreach (var pnj in _numberPnjOnGame)
        {
            pnj.agent.isStopped = !pnj.agent.isStopped;
        }
    }

    private void UpdateDayCounter()
    {
        dayCounter.text = "Day " + _numberDay;
    }

    private void UpdateTimeAndDay()
    {
        _numberSecondOfDay  += Time.deltaTime;
        _SecondForBirthCounter += Time.deltaTime;
        if  (_SecondForBirthCounter >= _numberSecondForABirth)
        {
            CreatePnj();
            _SecondForBirthCounter = 0f;
        }
        if (_numberSecondOfDay > _dayDuration) NextDay();
    }


    /*private void CreateBuilding(GameObject building, Transform positionOfBuilding, int numberOfMasonNeeded) //Give work to mason when a building is create
    {
        for (int i = 0; i < numberOfMasonNeeded; i++)
        {
            Character pnj = _numberMason[i].GetComponent<Character>();
            if (!pnj.isOccupied)
            {
                pnj.isOccupied = true;
                pnj.agent.SetDestination(positionOfBuilding.position);
            }
        }
        //Instantiate(building, positionOfBuilding.position, building.transform.rotation); //Faire en sorte que ça se fasse au bout de X temps
        //UpdateProsperity(); if Bookstore or Museum
    }*/

    /*private void CreateHome(GameObject home)
    {
        Instantiate(home);
        homes.Add(home);
    }*/

    private void CreatePnj()
    {
        _numberPnjOnGame.Add(Instantiate(wanderer).GetComponent<Character>());
    }

    private void NourrishPnj()
    {
        if (_numberFood < _numberPnjOnGame.Count)
        {
            int numberPnjToKill = Math.Abs(_numberFood - _numberPnjOnGame.Count);
            _numberFood = 0;
            MakeRandomPnjHungry(numberPnjToKill);
        }
        else _numberFood -= _numberPnjOnGame.Count;
    }

    private void MakeRandomPnjHungry(int numberToKill)  //Function call if there is more pnj than food so some will not be able to eat
    {
        for (int i = 0; i < numberToKill; i++)
        {
            int rand = Random.Range(0, _numberPnjOnGame .Count - 1);
            Character pnj = _numberPnjOnGame[rand];
            pnj.hunger = true;
        }
    }

    private void UpdateResources()
    {
        foreach (var character in _numberPnjOnGame)
        {
            switch (character.job)
            {
                case "farmer":
                    _numberFood += character.resourcesToGive;
                    break;
                case "lumberjack":
                    _numberWood +=  character.resourcesToGive;
                    break;
                case "miner":
                    _numberStone  += character.resourcesToGive;
                    break;
            }
            character.resourcesToGive = 0;
        }
    }
}
