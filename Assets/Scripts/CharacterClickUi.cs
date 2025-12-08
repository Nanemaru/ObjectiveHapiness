using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterClickUI : MonoBehaviour
{
    public GameObject jobUI; // Le panneau UI cach� au d�but
    private Camera cam;
    //[SerializeField] private CharacterJobAssign scriptJobAssign;
    private Character scriptCharacter;

    void Start()
    {
        cam = Camera.main;
        jobUI.SetActive(false); // UI cach� au lancement
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
            // Si l�objet cliqu� a le tag Character
            if (hit.collider.CompareTag("Character"))
            {
                jobUI.SetActive(true);
                scriptCharacter = hit.transform.gameObject.GetComponent<Character>();
                EnabledAllWorkButtonExceptOne(scriptCharacter.job);
            }
        }
    }
    
    private void EnabledAllWorkButtonExceptOne(string work)
    {
        foreach (Transform child in jobUI.transform)
        {
            Button button = child.GetComponent<Button>();
            if  (child.gameObject.name != work) button.interactable = true;
            else button.interactable = false;
        }
    }
    
    public void GiveANewJob(string newWork)
    {
        scriptCharacter.job =  newWork;
        jobUI.SetActive(false);
    }
}
