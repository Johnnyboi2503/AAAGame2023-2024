using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioZone : MonoBehaviour
{
    AudioSource audioSource;
    public float fadeDuration = 1.0f; // adjust to change the fade duration
    public float fullVolume = 1.0f; //adjust tp change how loud the volume should go up to (from 0 to 1)
    private bool isFading = false; // To track if fading is ongoing
    private bool playerIsInZone = false;
    private bool hasPlayedOnce = false;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f;
        audioSource.clip.LoadAudioData(); //load the data to prevent jitters upon entering a new zone
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFading) //so that there's nothing weird when quickly entering, leaving, and rentering an audio zone, we check if !isFading
        {
            playerIsInZone = true;
            StopAllCoroutines(); // Stop ongoing fade out coroutine


            if(!hasPlayedOnce) //this will ensure that the audio will only "start" during the first time entering a zone
            {
                audioSource.Play();
                hasPlayedOnce=true;
            }
           
            isFading = true;
            FadeIn();
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            playerIsInZone = false;
            StopAllCoroutines(); // stop ongoing fade in coroutine
            isFading = true;
            FadeOut();
        }
    }


    public void FadeIn()
    {
        StartCoroutine(StartFade(audioSource, fadeDuration, fullVolume)); // fade to full volume
    }


    public void FadeOut()
    {
        StartCoroutine(StartFade(audioSource, fadeDuration, 0f)); // fade to 0
    }


    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;


        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float masterVolume = AudioManager.GetInstance().GetMasterVolume();
            float musicVolume = AudioManager.GetInstance().GetMusicVolume();
            audioSource.volume = Mathf.Lerp(start, targetVolume * masterVolume * musicVolume, currentTime / duration);
            yield return null;
        }


        isFading = false;
        yield break;
    }


    public void UpdateVolumeBasedOnMasterVolume(){
        if(playerIsInZone){
            float masterVolume = fullVolume * AudioManager.GetInstance().GetMasterVolume();
            float musicVolume = fullVolume * AudioManager.GetInstance().GetMusicVolume();
            audioSource.volume = fullVolume * masterVolume * musicVolume;
        }
        else if (!isFading){
            audioSource.volume = 0f;
        }
    }
}
