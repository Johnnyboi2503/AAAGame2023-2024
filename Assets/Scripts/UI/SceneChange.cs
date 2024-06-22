using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour {

    [SerializeField] string sceneName;
    [SerializeField] float loadScreenTimerLength = 2f;
    [SerializeField] private float uiInteractionAudioVolume = 0.75f;
    public void ChangeScene() {
        TransitionUI.instance.StartTransition(loadScreenTimerLength, 
            () => // ON FINISHED FADE IN
            {
                SceneManager.LoadScene(sceneName);
            }, 
            () => // ON FINISHED FADE OUT
            {
                // Nothing needed
            });
    }
    
    public void ChangeSceneNoFade() {
        // Play UI Interaction Audio
        AudioSource audioSource = AudioManager.GetInstance().PlayGlobalAudio("UI_Interaction_SFX", uiInteractionAudioVolume);
        DontDestroyOnLoad(audioSource.transform.gameObject);
        SceneManager.LoadScene(sceneName);
    }
}
