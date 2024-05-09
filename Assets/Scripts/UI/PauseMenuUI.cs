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
        playerInput.EnableAbilityInput();
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        AudioManager.GetInstance().PauseAllAudio();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        playerInput.DisableAbilityInput();
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

