using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private GameManager _gameManager;
    
    //Data
    public string type;
    public List<int> cost; //order: wood, rock, mason
    private int prosperityToAdd;

    public void UpdateBuildEffect()
    {
        _gameManager = FindObjectOfType<GameManager>();
        switch (type)
        {
            case "Home":
                _gameManager.homes.Add(GetComponent<HomeClass>());
                break;
            case "Farm":
                _gameManager.foodMultiplicator += 1;
                break;
            case "Library":
                prosperityToAdd = 10;
                _gameManager.UpdateProsperity(prosperityToAdd);
                break;
            case "Museum":
                prosperityToAdd = 20;
                _gameManager.UpdateProsperity(prosperityToAdd);
                break;
            case "School":
                _gameManager.school = transform;
                _gameManager._isSchoolCreate =  true;
                break;
        }
    }
}
