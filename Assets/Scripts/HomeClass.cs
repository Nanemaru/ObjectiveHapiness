
using UnityEngine;

public class HomeClass : MonoBehaviour
{
    private bool _isAvailable = true;
    public bool IsAvailable
    {
        get => _isAvailable;
        set => _isAvailable = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
