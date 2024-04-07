using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseSliderValueToText : MonoBehaviour
{
    public Slider slider; 
    private TMP_Text tmpText; 

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>(); 
    }

    void Start()
    {
        if (slider != null && tmpText != null)
        {
            //listener
            slider.onValueChanged.AddListener(delegate {UpdateText(slider.value);});
            UpdateText(slider.value);
        }
    }
    void UpdateText(float value)
    {
        tmpText.text = value.ToString("F2");
    }
}
