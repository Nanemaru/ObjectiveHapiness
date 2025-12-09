using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private GameManager _gameManager;
    
    //Data
    public string type;
    public List<int> cost; //order: wood, rock, mason
    

    public void UpdateBuildEffect()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        switch (type)
        {
            case "Home":
                _gameManager.homes.Add(GetComponent<HomeClass>());
                break;
            case "Farm":
                _gameManager.foodMultiplicator += 1;
                break;
            case "Library":
                _gameManager.prosperity += 1;
                break;
            case "Museum":
                _gameManager.prosperity += 2;
                break;
        }
    }
}
