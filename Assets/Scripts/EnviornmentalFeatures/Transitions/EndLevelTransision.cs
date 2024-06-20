using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndLevelTransision : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] GameObject menuUI;
    [SerializeField] TMP_Text timeLogTests;

    private CursorLockMode prevCursorLockMode;
    private bool prevCursorVisibility;

    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent(out PlayerInput input)) {
            ActivateMenu();
        }
    }
    void ActivateMenu(){
        menuUI.SetActive(true);
        EndLevelObserver.NotifyEndLevel();
        Time.timeScale = 0f;

        prevCursorVisibility = Cursor.visible;
        prevCursorLockMode = Cursor.lockState;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadScene(string sceneName) {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    void OnEnable(){
        FinalCheckpointTimeLogObserver.FinalCheckpointTimeLog += OnFinalCheckpointTimeLog;
    }
    void OnDisable(){
        FinalCheckpointTimeLogObserver.FinalCheckpointTimeLog -= OnFinalCheckpointTimeLog;
    }

    void OnFinalCheckpointTimeLog(List<string> checkpointTimes){
        string timeLog = "";
        foreach(string timeString in checkpointTimes){
            timeLog += timeString + "\n";
        }
        timeLogTests.text = timeLog;
    }
}
