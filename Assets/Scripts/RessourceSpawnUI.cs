using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RessourceSpawnUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject RessourceSpawn;
    public void OnPointerEnter(PointerEventData eventData)
    {
       RessourceSpawn.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RessourceSpawn.SetActive(false);
    }
}
