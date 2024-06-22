using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Stopwatch : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;
    [SerializeField] StopwatchRecord record;

    [Header("Timer Settings")]
    public float currentTime;
    public bool timeActive;


    List<string> checkpointLogs = new List<string>();
    int checkpointCount = 0;

    [SerializeField] bool timerPersistsAfterRestarting = true;

    float previousCheckpointTimeState;


    void Start()
    {
        currentTime = 0;
        StartTime();
    }

    void Update()
    {
        if (timeActive == true)
        {
            currentTime += Time.deltaTime;
        }

        UpdateTimer();
    }



    public void StartTime()
    {
        timeActive = true;
    }

    public void StopTime()
    {
        timeActive = false;
    }

    void OnEnable(){
        CheckpointEnterObserver.CheckpointEnter += OnCheckpointEnter;
        EndLevelObserver.EndLevel += OnEndLevel;
        RestartAtCheckpointObserver.RestartAtCheckpoint += OnRestartAtCheckpoint;
    }
    void OnDisable(){
        CheckpointEnterObserver.CheckpointEnter -= OnCheckpointEnter;
        EndLevelObserver.EndLevel -= OnEndLevel;
        RestartAtCheckpointObserver.RestartAtCheckpoint -= OnRestartAtCheckpoint;
    }

    void OnCheckpointEnter(){
        checkpointCount++;
        checkpointLogs.Add("Checkpoint " + checkpointCount + ": " + timerText.text);
        Debug.Log(checkpointLogs[checkpointCount - 1]);
        previousCheckpointTimeState = currentTime;
    }

    void OnEndLevel(){
        checkpointCount++;
        UpdateTimer();
        record.SaveTime(SceneManager.GetActiveScene().name, currentTime);
        checkpointLogs.Add("Final Time: " + timerText.text);
        Debug.Log(checkpointLogs[checkpointCount - 1]);
        FinalCheckpointTimeLogObserver.NotifyFinalCheckpointTimeLog(checkpointLogs);
    }

    void OnRestartAtCheckpoint(){
        if(!timerPersistsAfterRestarting){
            currentTime = previousCheckpointTimeState;
        }
    }

    void UpdateTimer() {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timerText.text = time.ToString(@"mm\:ss\:fff");
    }
}
