using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointEnterObserver : MonoBehaviour
{
    public delegate void OnCheckpointEnter();
    public static event OnCheckpointEnter CheckpointEnter;

    public static void NotifyCheckpointEnter()
    {
        CheckpointEnter?.Invoke();
    }

    /*
    void OnEnable(){
        CheckpointObserver.CheckpointEnter += OnCheckpointEnter;
        }
    void OnDisable(){
        CheckpointObserver.CheckpointEnter -= OnCheckpointEnter;
    }

    void OnCheckpointEnter(){
        Debug.Log("Checkpoint Entered");
    }
    */
}