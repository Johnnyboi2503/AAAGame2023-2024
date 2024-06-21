using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioZone : MonoBehaviour
{
    enum ZoneType
    {
        MainMenuMusic,
        StageMusic,
        PauseMusic,
        DialogueMusic,
    }
    [SerializeField] ZoneType zoneType;

    AudioSource audioSource;
    public float fadeDuration = 1.0f; // adjust to change the fade duration
    public float fullVolume = 1.0f; //adjust tp change how loud the volume should go up to (from 0 to 1)
    private bool isFading = false; // To track if fading is ongoing
    private bool playerIsInZone = false;
    private bool hasPlayedOnce = false;

    bool mainMenuIsFadingOut = false;

    [SerializeField] GameObject triggerObject; // The object that will trigger the audio to play


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f;
        audioSource.clip.LoadAudioData(); //load the data to prevent jitters upon entering a new zone
        if(zoneType == ZoneType.PauseMusic || zoneType == ZoneType.DialogueMusic){
            audioSource.volume = fullVolume;
            audioSource.Pause();
        }
        if(zoneType == ZoneType.MainMenuMusic){
            audioSource.volume = fullVolume;
        }
    }

    void Update()
    {
        // Check if the player is inside the trigger object
        if (triggerObject != null && triggerObject.GetComponent<Collider>().bounds.Contains(transform.position) && !mainMenuIsFadingOut)
        {
            if (!playerIsInZone && !isFading)
            {
                playerIsInZone = true;
                StopAllCoroutines(); // Stop ongoing fade out coroutine

                if (!hasPlayedOnce) 
                {
                    audioSource.Play();
                    hasPlayedOnce = true;
                }

                isFading = true;
                FadeIn();
            }
        }
        else
        {
            if (playerIsInZone && !isFading)
            {
                playerIsInZone = false;
                StopAllCoroutines(); // stop ongoing fade in coroutine
                isFading = true;
                FadeOut();
            }
        }
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
        if(zoneType == ZoneType.PauseMusic || zoneType == ZoneType.DialogueMusic) 
            audioSource.Pause();
        StartCoroutine(StartFade(audioSource, fadeDuration, fullVolume)); // fade to full volume
    }


    public void FadeOut()
    {
        //i'm just turning off this functionality bc it doesn't seem like we'll need it and it's causing issues upon player death.
        //StartCoroutine(StartFade(audioSource, fadeDuration, 0f)); // fade to 0

    }

    public void MainMenuFadeOut(){
        mainMenuIsFadingOut = true;
        StartCoroutine(StartFade(audioSource, fadeDuration, 0f));
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

    void OnEnable(){
        GamePauseChangeObserver.GamePauseChange += OnGamePauseChange;
        DialogueStateChangeObserver.DialogueStateChange += OnDialogueStateChange;
    }
    void OnDisable(){
        GamePauseChangeObserver.GamePauseChange -= OnGamePauseChange;
        DialogueStateChangeObserver.DialogueStateChange -= OnDialogueStateChange;
    }
    void OnGamePauseChange(bool newPauseState){
        if(zoneType == ZoneType.StageMusic){
            if(newPauseState == true){
                audioSource.Pause();
            }
            else{
                audioSource.UnPause();
            }
        }
        else if(zoneType == ZoneType.PauseMusic){
            if(newPauseState == true){
                audioSource.UnPause();
            }
            else{
                audioSource.Pause();
            }
        }
    }
    void OnDialogueStateChange(bool dialogueActive){
        if(zoneType == ZoneType.StageMusic){
            if(dialogueActive == true){
                audioSource.Pause();
            }
            else{
                audioSource.UnPause();
            }
        }
        if(zoneType == ZoneType.DialogueMusic){
            if(dialogueActive){
                audioSource.UnPause();
            }
            else{
                audioSource.Pause();
            }
        }
    }
}
