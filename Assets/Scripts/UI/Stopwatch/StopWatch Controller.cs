using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWatchController : MonoBehaviour
{

    public Stopwatch stopwatch;

    public bool timerStarter;
    public bool timerEnder;

    private void Start() {
        stopwatch = FindObjectOfType<Stopwatch>();
    }
    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Player"))
       {
            if(timerStarter) //this is set to true in the inspector for the object that starts the timer
            {
                stopwatch.StartTime();
            }


            if(timerEnder) //this is set to true in the inspector for the object that ends the timer
            {
                stopwatch.StopTime();
            }
       }
    }

   


}
