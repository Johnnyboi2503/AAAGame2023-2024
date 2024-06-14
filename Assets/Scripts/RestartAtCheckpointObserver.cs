using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartAtCheckpointObserver : MonoBehaviour
{
    public delegate void OnRestartAtCheckpoint();
    public static event OnRestartAtCheckpoint RestartAtCheckpoint;

    public static void NotifyRestartAtCheckpoint()
    {
        RestartAtCheckpoint?.Invoke();
    }

    /*
    void OnEnable(){
        RestartAtCheckpointObserver.RestartAtCheckpoint += OnRestartAtCheckpoint;
    }
    void OnDisable(){
        RestartAtCheckpointObserver.RestartAtCheckpoint -= OnRestartAtCheckpoint;
    }

    void OnRestartAtCheckpoint(){
        Debug.Log("Restart at Checkpoint");
    }
    */

}
