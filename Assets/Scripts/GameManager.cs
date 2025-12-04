using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    //Time management
    public float _prosperity = 5f;
    private float _dayDuration = 5f; //Set the duration in seconds of a day 
    private float _numberSecondOfDay = 0f;
    private int _numberDay = 1;
    private bool _isOnPlay = true; //Value to use to put game in resume
    public bool IsOnPlay
    {
        get => _isOnPlay;
        set => _isOnPlay = value;
    }
    
    private float _numberSecondBeforeBirth = 30f;
    
    //Define how much resource there is in game
    private static int _numberFood = 0; 
    private static int _numberWood = 0;
    private static int _numberStone = 0;
    public float foodMultiplicator = 1;
    private int[] _resourcesIntArray = new int[3] { _numberFood, _numberWood, _numberStone };
    //Canvas management
        [SerializeField] private TextMeshProUGUI[] resourcesTextArray = new TextMeshProUGUI[3];
        [SerializeField] private TextMeshProUGUI dayCounter;
        
    //PNJ
    [SerializeField] private List<GameObject> typeOfPnj = new List<GameObject>(); //Put wanderer in first
    public List<Character> _numberPnjOnGame = new List<Character>();
    private List<GameObject> _numberMason = new List<GameObject>();

    //Buildings
    public List<GameObject> homes = new List<GameObject>();

    public Transform school;
    public bool _isSchoolCreate = false;
    // Start is called before the first frame update
    void Start()
    {
       UpdateDayCounter();
    }

    // Update is called once per frame
    void Update()
    {
        while (_isOnPlay)
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
        NourrishPnj();
        UpdatePnj();
        UpdateDayCounter();
    }

    private void UpdatePnj()
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

    private void UpdateResourcesText()
    {
        for (int i = 0; i < resourcesTextArray.Length; i++)
        {
            resourcesTextArray[i].text = _resourcesIntArray[i].ToString();
        }
    }

    private void UpdateDayCounter()
    {
        dayCounter.text = "Day " + _numberDay;
    }

    private void UpdateTimeAndDay()
    {
        _numberSecondOfDay  += Time.deltaTime;
        if (_numberSecondOfDay > _numberSecondBeforeBirth) CreatePnj();
        if (_numberSecondOfDay > _dayDuration) NextDay();
    }


    private void CreateBuilding(GameObject building, Transform positionOfBuilding, int numberOfMasonNeeded) //Give work to mason when a building is create
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
    }

    private void CreateHome(GameObject home)
    {
        Instantiate(home);
        homes.Add(home);
    }

    private void CreatePnj()
    {
        int rand =  Random.Range(0, typeOfPnj.Count - 1);
        GameObject newPnj = Instantiate(typeOfPnj[rand]);
        Character scriptNewPnj = newPnj.GetComponent<Character>();
        _numberPnjOnGame.Add(scriptNewPnj);
        scriptNewPnj.job = "wanderer";
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
            Character pnj = _numberPnjOnGame[rand].GetComponent<Character>();
            pnj.hunger = true;
        }
    }
}
