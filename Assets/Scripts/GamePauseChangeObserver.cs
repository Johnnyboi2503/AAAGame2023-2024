using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseChangeObserver : MonoBehaviour
{
    public delegate void OnGamePauseChange(bool isPaused);
    public static event OnGamePauseChange GamePauseChange;

    public static void NotifyGamePauseChange(bool isPaused)
    {
        GamePauseChange?.Invoke(isPaused);
    }

    /*
    void OnEnable(){
        GamePauseChangeObserver.GamePauseChange += OnGamePauseChange;
    }
    void OnDisable(){
        GamePauseChangeObserver.GamePauseChange -= OnGamePauseChange;
    }

    void OnGamePauseChange(bool isPaused){
        Debug.Log("Game is paused: " + isPaused);
    }
    */

}
