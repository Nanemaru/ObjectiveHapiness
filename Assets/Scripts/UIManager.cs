using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textFood;
    [SerializeField] TextMeshProUGUI textWood;
    [SerializeField] TextMeshProUGUI textStone;
    [SerializeField] GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textFood.text = _gameManager._numberFood.ToString();
        textWood.text = _gameManager._numberWood.ToString();
        textStone.text = _gameManager._numberStone.ToString();
    }
}
