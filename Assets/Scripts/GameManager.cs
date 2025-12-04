using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Time management
    private float _prosperity = 5f;
    private float _dayDuration = 5f; //Set the duration in seconds of a day 
    private float _numberSecondOfDay = 0f;
    private int _numberDay = 1;
    private bool _isOnPlay = true; //Value to use to put game in resume
    private float _numberSecondBeforeBirth = 30f;
    
    //Define how much resource there is in game
    private static int _numberFood = 0; 
    private static int _numberWood = 0;
    private static int _numberStone = 0;
    private int[] _resourcesIntArray = new int[3] { _numberFood, _numberWood, _numberStone };
    //Canvas management
        [SerializeField] private TextMeshProUGUI[] resourcesTextArray = new TextMeshProUGUI[3];
        [SerializeField] private TextMeshProUGUI dayCounter;
        
    //PNJ
    [SerializeField] private List<GameObject> typeOfPnj = new List<GameObject>(); //Put wanderer in first
    public List<GameObject> _numberPnjOnGame = new List<GameObject>();

    private enum Job {farmer, lumberjack, miner, mason};
    //Buildings
    public List<GameObject> homes = new List<GameObject>();
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
        UpdatePnj();
        UpdateDayCounter();
    }

    private void UpdatePnj()
    {
        foreach (GameObject character in _numberPnjOnGame)
        {
            Character pnj = character.GetComponent<Character>();
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


    private void CreateBuilding(GameObject building, Transform positionOfBuilding)
    {
        Instantiate(building, positionOfBuilding.position, building.transform.rotation);
    }

    private void CreateHome(GameObject home)
    {
        Instantiate(home);
        homes.Add(home);
    }

    private void CreatePnj()
    {
        int rand =  Random.Range(0, typeOfPnj.Count - 1);
        GameObject pnj = Instantiate(typeOfPnj[rand]);
        _numberPnjOnGame.Add(pnj);
        pnj.GetComponent<Character>().job = "wanderer";
    }
}
