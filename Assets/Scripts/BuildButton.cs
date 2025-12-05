using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
    public GameObject buildingPrefab;
    public Building buildingData;
    public BuildingPlacer placer;

    public void OnClick()
    {
        placer.StartPlacing(buildingPrefab, buildingData);
    }
}


