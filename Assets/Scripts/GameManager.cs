using System.Collections;
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
    
    //Define how much resource there is in game
    private static int _numberFood = 0; 
    private static int _numberWood = 0;
    private static int _numberStone = 0;
    private int[] _resourcesIntArray = new int[3] { _numberFood, _numberWood, _numberStone };
    //Canvas management
        //Resources Text
        [SerializeField] private TextMeshProUGUI[] resourcesTextArray = new TextMeshProUGUI[3];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeAndDay();
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
    }

    public void UpdateProsperity(float value) //Function call to update prosperity with positive or negative value
    {
        _prosperity += value;
        if (_prosperity < 0) _prosperity = 0f;
    }

    public void Resume() //Put game from play to resume and vice versa
    {
        _isOnPlay = !_isOnPlay;
    }

    private void UpdateResourcesText()
    {
        for (int i = 0; i < resourcesTextArray.Length; i++)
        {
            resourcesTextArray[i].text = _resourcesIntArray[i].ToString();
        }
    }

    private void UpdateTimeAndDay()
    {
        _numberSecondOfDay  += Time.deltaTime;
        if (_numberSecondOfDay > _dayDuration) NextDay();
    }
}
