using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public class CharacterJobAssign : MonoBehaviour
{
    private GameObject character;
    private CharacterClickUI characterClick;
    public Character characterData;
    [SerializeField] private string JobAssign;
    public Button button;
    
    private void Start()
    {
        
    }
    void BuildButton()
    {
        character = characterClick.CharacterClicked; //Assignation du GameObject du personnage à Character
        Debug.Log(character);
        characterData = character.GetComponent<Character>();
        bool canBuild = JobAssign != characterData.job;
        button.interactable = canBuild;
    }

    public void OnClick()
    {
        BuildButton();
        Debug.Log("test");
        characterData.job = JobAssign;
    }
}
