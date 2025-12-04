using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string type;
    public List<int> cost; //order: wood, rock, mason

    void Start()
    {
        switch (type)
        {
            case "Home":
                GameManager.homeList.Add(4);
                break;
            case "Farm":
                GameManager.foodMultiplicator += 0.2;
                break;
            case "Library":
                GameManager.Prosperity += 1;
                break;
            case "Museum":
                GameManager.Prosperity += 2;
                break;
        }
    }
    void Update()
    {
        
    }
}
