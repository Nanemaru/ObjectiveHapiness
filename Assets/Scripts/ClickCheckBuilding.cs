using UnityEngine;
using UnityEngine.UI;

public class ClickCheckBuilding : MonoBehaviour
{
    public Building building;              
    public Button buildButton;             
    public GameManager gameManager;

    void Update() //Enabled or disabled a construction button if a construction is possible or not
    {
        bool canBuild = gameManager._numberWood >= building.cost[0] && gameManager._numberStone >= building.cost[1] && gameManager._numberMason >= building.cost[2];
        buildButton.interactable = canBuild;
    }
}