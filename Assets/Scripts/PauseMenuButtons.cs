using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButtons : MonoBehaviour
{
    public GameObject optionsUI;        
    public GameObject buttonsGroup;     

    public void GoToOptions()
    {
        optionsUI.SetActive(true);      
        buttonsGroup.SetActive(false);
    }

    public void LeaveOptions()
    {
        optionsUI.SetActive(false);     
        buttonsGroup.SetActive(true);   
    }
}
