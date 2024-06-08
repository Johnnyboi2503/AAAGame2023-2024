using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject playButton;

    void Start(){
        //sets the initial selection on start
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    void Update()
    { 
        if (EventSystem.current.currentSelectedGameObject == null && AnyKeyboardOrControllerInputDetected())
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(playButton);
        }
    }



    bool AnyKeyboardOrControllerInputDetected()
    {
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
        {
            return true;
        }

        // Check for controller input
        if (Input.GetAxis("Left Stick Vertical") != 0 || Input.GetAxis("Left Stick Horizontal") != 0 || Input.GetButtonDown("Submit"))
        {
            return true;
        }

        return false;
    }
}
