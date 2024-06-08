using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class PauseMenuUI : MonoBehaviour
{
    private PlayerKillable playerKillable;
    private PlayerInput playerInput;
    public GameObject pauseMenuUI;
    private bool gameIsPaused = false;
    private bool optionsMenuIsOpen = false;

    public KeyCode pauseKeyKeyboard;

    private CursorLockMode prevCursorLockMode;
    private bool prevCursorVisibility;

    [SerializeField] GameObject resumeButton;
    [SerializeField] GameObject optionsMenuReturnButton;

    [SerializeField] GameObject optionsUI;        
    [SerializeField] GameObject buttonsGroup;     

    [Space]
    [SerializeField] private float uiInteractionAudioVolume = 0.75f;
    private void OnEnable()
    {
     playerKillable = FindObjectOfType<PlayerKillable>();
     playerInput = FindAnyObjectByType<PlayerInput>();
    }

    private void Update()
    {
        //This may not work for Xbox and Nintendo controllers since i dont think they use JoystickButton 9 as their pause button
        if(Input.GetKeyDown(pauseKeyKeyboard)|| Input.GetKeyDown(KeyCode.JoystickButton9))
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

        if(optionsMenuIsOpen){
            if (EventSystem.current.currentSelectedGameObject == null && AnyKeyboardOrControllerInputDetected())
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(optionsMenuReturnButton);
            }
        }
        else if (gameIsPaused)
        {
            if (EventSystem.current.currentSelectedGameObject == null && AnyKeyboardOrControllerInputDetected())
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(resumeButton);
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

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);

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

    bool AnyKeyboardOrControllerInputDetected()
    {
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
        {
            return true;
        }

        // Check for controller input
        if (Input.GetAxis("Left Stick Vertical") != 0 || Input.GetAxis("Left Stick Horizontal") != 0 || Input.GetButtonDown("Submit"))
        {
            return true;
        }

        return false;
    }


    public void GoToOptions()
    {
        EventSystem.current.SetSelectedGameObject(null);
        optionsMenuIsOpen = true;
        optionsUI.SetActive(true);      
        buttonsGroup.SetActive(false);
    }

    public void LeaveOptions()
    {
        EventSystem.current.SetSelectedGameObject(null);
        optionsMenuIsOpen = false;
        optionsUI.SetActive(false);     
        buttonsGroup.SetActive(true);   
    }
}


