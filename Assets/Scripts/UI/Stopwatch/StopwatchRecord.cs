using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StopwatchRecord", order = 1)]
public class StopwatchRecord : ScriptableObject {
    public Dictionary<string, float> stopwatchRecord = new Dictionary<string, float>();

    [ContextMenu("Print Data")]
    public void PrintData() {
        Debug.Log("printing stopwatch data");
        foreach(KeyValuePair<string, float> pair in stopwatchRecord) {
            Debug.Log(pair.Key + "---" + pair.Value);
        }
    }
    [ContextMenu("Clear Data")]
    public void ClearData() {
        stopwatchRecord.Clear();
    }

    public void SaveTime(string sceneName, float time) {
        // Saving new value
        if(stopwatchRecord.ContainsKey(sceneName)) {
            if(stopwatchRecord[sceneName] > time) {
                stopwatchRecord[sceneName] = time;
            }
        }
        else {
            stopwatchRecord[sceneName] = time;
        }
    }
}
