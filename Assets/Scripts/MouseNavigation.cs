using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseNavigation : MonoBehaviour
{
    public float ScrollSpeed = 15;
// Update is called once per frame
    void Update()
    {				             
        if (Input.mousePosition.y >= Screen.height *0.95) MoveCamera(Vector3.forward);
        if (Input.mousePosition.y <= Screen.height *0.05) MoveCamera(Vector3.back);
        if (Input.mousePosition.x >= Screen.width *0.95) MoveCamera(Vector3.right);
        if (Input.mousePosition.x <= Screen.width *0.05) MoveCamera(Vector3.left);
    }

    private void MoveCamera(Vector3 vectorDirection)
    {
        transform.Translate(vectorDirection * (Time.deltaTime * ScrollSpeed), Space.World);
    }
}
