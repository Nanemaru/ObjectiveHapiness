
using UnityEngine;

public class HomeClass : MonoBehaviour
{
    private bool _isAvailable = true;
    public bool IsAvailable
    {
        get => _isAvailable;
        set => _isAvailable = value;
    }

    private int _numberBedLeft = 4;

    public int NumberBedLeft
    {
        get => _numberBedLeft;
        set => _numberBedLeft = value;
    }

    private void OnTriggerEnter(Collider other) //Disable pnj renderer when they're in their home
    {
        if (other.CompareTag("Character")) other.GetComponent<MeshRenderer>().enabled = false;
    }
    
    private void OnTriggerExit(Collider other) //Enable pnj renderer when they're in their home
    {
        if (other.CompareTag("Character")) other.GetComponent<MeshRenderer>().enabled = false;
    }
}
