using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioZone : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] string audioClipName;
    [SerializeField] float fadeInDuration = 1.0f; // adjust to change the fade duration
    [SerializeField] float fadeOutDuration = 1.0f;
    [SerializeField] float volume = 1.0f; //adjust tp change how loud the volume should go up to (from 0 to 1)
    
    bool isFading = false; // To track if fading is ongoing
    bool hasPlayedOnce = false;

    //void Start()
    //{
        //audioSource = GetComponent<AudioSource>();
        //audioSource.volume = 0f; 
        //audioSource.clip.LoadAudioData(); //load the data to prevent jitters upon entering a new zone
    //}


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFading) //so that there's nothing weird when quickly entering, leaving, and rentering an audio zone, we check if !isFading
        {
            StopAllCoroutines(); // Stop ongoing fade out coroutine
            isFading = true;

            if(!hasPlayedOnce) //this will ensure that the audio will only "start" during the first time entering a zone
            {
                audioSource = AudioManager.GetInstance().PlayGlobalAudio(audioClipName, volume, true, fadeInDuration);
                hasPlayedOnce = true;
                isFading = false;
            }
            else{
                FadeIn();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            StopAllCoroutines(); // stop ongoing fade in coroutine
            isFading = true;
            FadeOut();
        }
    }

    public void FadeIn()
    {
        StartCoroutine(StartFadeDontDestroy(audioSource, fadeInDuration, volume)); // fade to full volume

    }

    public void FadeOut()
    {
        StartCoroutine(StartFadeDontDestroy(audioSource, fadeOutDuration, 0f)); // fade to 0
        
    }

    public IEnumerator StartFadeDontDestroy(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        isFading = false;
        yield break;
    }
}