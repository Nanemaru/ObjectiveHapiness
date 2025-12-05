using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterClickUI : MonoBehaviour
{
    public GameObject CharacterClicked;
    public GameObject jobUI; // Le panneau UI caché au début
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        jobUI.SetActive(false); // UI caché au lancement
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            DetectCharacterClick();
        }
    }

    void DetectCharacterClick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Si l’objet cliqué a le tag Character
            if (hit.collider.CompareTag("Character"))
            {
                jobUI.SetActive(true);
                CharacterClicked = hit.transform.gameObject;
                
            }
        }
    }
}
