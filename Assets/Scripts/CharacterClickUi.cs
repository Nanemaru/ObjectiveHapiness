using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterClickUI : MonoBehaviour
{
    public GameObject jobUI;
    private Camera _cam;
    private Character _scriptCharacter;
    private GameManager _gameManager;
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _cam = Camera.main;
        jobUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            DetectCharacterClick();
        }
    }

    void DetectCharacterClick()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Character"))
            {
                jobUI.SetActive(true);
                _scriptCharacter = hit.transform.gameObject.GetComponent<Character>();
                EnabledAllWorkButtonExceptOne(_scriptCharacter.job);
            }
        }
    }
    
    //When a character is chosen for professional retraining, put the button of his work not interactable
    private void EnabledAllWorkButtonExceptOne(string work)
    {
        if (_gameManager._isSchoolCreate)
        {
            foreach (Transform child in jobUI.transform)
            {
                if (child.GetComponent<Button>())
                {
                    Button button = child.GetComponent<Button>();
                    button.interactable = child.gameObject.name != work;
                }
            }
        }
    }
    
    //Function who change the job of a character when they do a professional retraining
    public void GiveANewJob(string newWork)
    {
        _scriptCharacter.newJob =  newWork;
        _scriptCharacter.goingToSchool = true;
        _scriptCharacter.agent.destination = _gameManager.school.position;
        jobUI.SetActive(false);
    }
}
