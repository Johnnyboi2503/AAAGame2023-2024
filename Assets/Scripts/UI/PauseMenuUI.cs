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

    [Space]
    [SerializeField] private float uiInteractionAudioVolume = 0.75f;
    private void OnEnable()
    {
     playerKillable = FindObjectOfType<PlayerKillable>();
     playerInput = FindAnyObjectByType<PlayerInput>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(pauseKey))
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
        PlayUIInteractionAudio();

        ChangeScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        PlayUIInteractionAudio();

        playerKillable.TakeDamage(100000);

        // Reset prev Cursor State
        prevCursorVisibility = false;
        prevCursorLockMode = CursorLockMode.Locked;
        Resume();
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void PlayUIInteractionAudio()
    {
        AudioSource audioSource = AudioManager.GetInstance().PlayGlobalAudio("UI_Interaction_SFX", uiInteractionAudioVolume);
        DontDestroyOnLoad(audioSource.transform.gameObject);
    }
}


