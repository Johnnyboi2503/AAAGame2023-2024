using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelObserver : MonoBehaviour
{
    public delegate void OnEndLevel();

    public static event OnEndLevel EndLevel;

    public static void NotifyEndLevel()
    {
        EndLevel?.Invoke();
    }

    /*
    void OnEnable(){
        EndLeveObserver.EndLevel += OnEndLevel;
        }
    void OnDisable(){
        EndLeveObserver.EndLevel -= OnEndLevel;
    }
    void OnEndLevel(){
        Debug.Log("End Level");
    }
    */
}