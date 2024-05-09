using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    [SerializeField] private float uiInteractionAudioVolume = 0.75f;
    public void RestartGame()
    {
        // Play UI Interaction Audio
        AudioSource audioSource = AudioManager.GetInstance().PlayGlobalAudio("UI_Interaction_SFX", uiInteractionAudioVolume);
        DontDestroyOnLoad(audioSource.transform.gameObject);

        // KarenTestScene would be subbed in for whatever the scene name of the level is

        SceneManager.LoadScene("KarenTestScene");
    }
}
