using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string type;
    public List<int> cost; //order: wood, rock, mason
    private GameManager gameManager;

    void Start()
    {
        switch (type)
        {
            case "Home":
                gameManager.homes.Add(gameObject);
                break;
            case "Farm":
                gameManager.foodMultiplicator += 1;
                break;
            case "Library":
                gameManager._prosperity += 1;
                break;
            case "Museum":
                gameManager._prosperity += 2;
                break;
        }
    }
    void Update()
    {
        
    }
}
