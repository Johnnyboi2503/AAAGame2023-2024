using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Stopwatch : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    public float currentTime;
    public bool timeActive;


    void Start()
    {
        currentTime = 0;
    }

    void Update()
    {
        if (timeActive == true)
        {
            currentTime += Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timerText.text = time.ToString(@"mm\:ss\:fff");
    }

    public void StartTime()
    {
        timeActive = true;
    }

    public void StopTime()
    {
        timeActive = false;
    }
}
