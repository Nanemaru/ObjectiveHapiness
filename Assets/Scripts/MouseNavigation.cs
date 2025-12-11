using System;
using UnityEngine;

public class MouseNavigation : MonoBehaviour
{
    private float _scrollSpeed = 15;
    
    private CharacterClickUI  _characterClickUI;

    private void Start()
    {
        _characterClickUI = FindObjectOfType<CharacterClickUI>();
    }

    void Update()
    {
        //When mouse reach a border of the screen, call MoveCamera function with the vector of the direction
        if (!_characterClickUI.jobUI.activeSelf)
        {
            if (Input.mousePosition.y >= Screen.height * 0.95 && transform.position.z <= -125) MoveCamera(Vector3.forward);
            if (Input.mousePosition.y <= Screen.height * 0.05 && transform.position.z >= -200) MoveCamera(Vector3.back);
            if (Input.mousePosition.x >= Screen.width * 0.95 && transform.position.x <= 487) MoveCamera(Vector3.right);
            if (Input.mousePosition.x <= Screen.width * 0.05 && transform.position.x >= 433) MoveCamera(Vector3.left);
        }
    }

    private void MoveCamera(Vector3 vectorDirection) //Move camera in the direction put in parameter
    {
        transform.Translate(vectorDirection * (Time.deltaTime * _scrollSpeed), Space.World);
    }
}
