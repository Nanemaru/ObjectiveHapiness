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
    [SerializeField] TextMeshProUGUI dayCounter;
    [SerializeField] TextMeshProUGUI pnjCounter;
    [SerializeField] Slider sliderProsperity;
    
    [SerializeField] GameManager _gameManager;
    
    //Pop-up
    public VerticalLayoutGroup verticalLayoutGroup;
    public GameObject newPnj;
    public GameObject deadPnj;
    // Start is called before the first frame update
    void Start()
    {
        UpdateResourceText();
        UpdateDayCounter();
        UpdatePNJCounter();
    }

    public void UpdateResourceText()
    {
        textFood.text = _gameManager._numberFood.ToString();
        textWood.text = _gameManager._numberWood.ToString();
        textStone.text = _gameManager._numberStone.ToString();
    }
    
    public void UpdateDayCounter()
    {
        dayCounter.text = "Day " + _gameManager.numberDay;
    }

    public void UpdatePNJCounter()
    {
        pnjCounter.text = "Citizen: " + _gameManager.numberOfPnj;
    }

    public void UpdateSliderProsperity(float value)
    {
        sliderProsperity.value = value/100;
    }
    
    public void NewPopUp(GameObject popUp)
    {
        GameObject newPopUp = Instantiate(popUp, verticalLayoutGroup.transform);
        Destroy(newPopUp, 2f);
    }
 
}
