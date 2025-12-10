
using UnityEngine;

public class HomeClass : MonoBehaviour
{
    private bool _isAvailable = true;
    public bool IsAvailable
    {
        get => _isAvailable;
        set => _isAvailable = value;
    }

    private int _numberBedLeft = 2;

    public int NumberBedLeft
    {
        get => _numberBedLeft;
        set => _numberBedLeft = value;
    }
}
