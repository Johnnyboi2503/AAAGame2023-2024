using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCheckpointTimeLogObserver : MonoBehaviour
{
    public delegate void OnFinalCheckpointTimeLog(List<string> checkpointTimes);
    public static event OnFinalCheckpointTimeLog FinalCheckpointTimeLog;

    public static void NotifyFinalCheckpointTimeLog(List<string> checkpointTimes)
    {
        FinalCheckpointTimeLog?.Invoke(checkpointTimes);
    }

    /*
    void OnEnable(){
        FinalCheckpointTimeLogObserver.FinalCheckpointTimeLog += OnFinalCheckpointTimeLog;
    }
    void OnDisable(){
        FinalCheckpointTimeLogObserver.FinalCheckpointTimeLog -= OnFinalCheckpointTimeLog;
    }

    void OnFinalCheckpointTimeLog(List<string> checkpointTimes){
        Debug.Log("Final Checkpoint");
    }
    */
}