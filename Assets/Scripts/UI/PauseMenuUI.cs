using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    private PlayerKillable playerKillable;
    private PlayerInput playerInput;
    public GameObject pauseMenuUI;
    private bool gameIsPaused = false;
    public KeyCode pauseKey;

    private CursorLockMode prevCursorLockMode;
    private bool prevCursorVisibility;

    bool levelEnded = false;

    [Space]
    [SerializeField] private float uiInteractionAudioVolume = 0.75f;

    void Start()
    {
        Time.timeScale = 1;
    }
    private void OnEnable()
    {
        playerKillable = FindObjectOfType<PlayerKillable>();
        playerInput = FindAnyObjectByType<PlayerInput>();
        FinalCheckpointTimeLogObserver.FinalCheckpointTimeLog += OnFinalCheckpointTimeLog;
    }

    private void Update()
    {
        if(Input.GetKeyDown(pauseKey) && !levelEnded)
        {
            if(gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();    
            }
        }
    }
    public void Resume()
    {
        AudioManager.GetInstance().ResumeAllAudio();
        GamePauseChangeObserver.NotifyGamePauseChange(false);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;

        playerInput.EnableInput();
        // Reenable player ability input if the cursor mode is not free
        if (prevCursorLockMode != CursorLockMode.None)
        {
            playerInput.EnableAbilityInput();
        }
        
        Cursor.visible = prevCursorVisibility; 
        Cursor.lockState = prevCursorLockMode;
    }

    public void Pause()
    {
        AudioManager.GetInstance().PauseAllAudio();
        GamePauseChangeObserver.NotifyGamePauseChange(true);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        
        playerInput.DisableInput();
        playerInput.DisableAbilityInput();

        prevCursorVisibility = Cursor.visible;
        prevCursorLockMode = Cursor.lockState;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void QuitToMenu()
    {
        ChangeScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        // Reset prev Cursor State
        prevCursorVisibility = false;
        prevCursorLockMode = CursorLockMode.Locked;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void OnDisable(){
        FinalCheckpointTimeLogObserver.FinalCheckpointTimeLog -= OnFinalCheckpointTimeLog;
    }

    void OnFinalCheckpointTimeLog(List<string> checkpointTimes){
        levelEnded = true;
    }
}


