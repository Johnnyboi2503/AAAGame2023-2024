using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//TODO:
    //ADD A METHOD TO UPDATE ALL CURRENTLY-PLAYING AUDIO SOURCES WHEN MASTERVOLUME GETS CHANGED BY THE PLAYER




public class AudioManager : MonoBehaviour
{
    //singleton
    static AudioManager instance;
    public static AudioManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<AudioManager>();


            if (instance == null)
            {
                GameObject obj = new GameObject("AudioManager");
                instance = obj.AddComponent<AudioManager>();
            }
        }
        return instance;
    }


    //dictionaries are a collection of items with two values, a key and a value
    //in this context, we're creating a dictionary called audioClips,
    //where the values are the audioclip, and the key is the string that points to it


    private Dictionary<string, AudioClip> audioClips;
    private Dictionary<string, List<AudioSource>> playingAudioSources;
    private Dictionary<AudioSource, float> originalVolumes;


    [SerializeField]float MasterVolume = 1.0f;
    [SerializeField]float SFXVolume = 1.0f;
    [SerializeField]float MusicVolume = 1.0f;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }


        playingAudioSources = new Dictionary<string, List<AudioSource>>();
        originalVolumes = new Dictionary<AudioSource, float>();


        LoadAudioClips();
    }


    void Update(){
        CleanupInvalidAudioSources();




    }


    //thr current implementation depends on there being an "Audio" Folder inside of a
    //"Resources" folder to take advantage of the Resources.LoadAll method
    void LoadAudioClips()
    {
        //initializes a new, empty dictionary
        audioClips = new Dictionary<string, AudioClip>();


        //declares an array of AudioClips, takes advantage of the
        //Resources.LoadAll method to add all audioclips into the array
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");


        //for every audioclip in the array, create an entry in the dictionary
        foreach(var clip in clips){
            audioClips.Add(clip.name, clip);
        }
    }




   
    public AudioSource PlayGlobalAudio(string key, float volume = 1.0f, bool loop = false, float fadeTime = 0f)
    {
        if (audioClips.ContainsKey(key))
        {
            GameObject audioObject = new GameObject("AudioObject_" + key);
            AudioSource source = audioObject.AddComponent<AudioSource>();


            originalVolumes.Add(source, volume);


            source.clip = audioClips[key];
            source.volume = 0;
            source.loop = loop;
            source.spatialBlend = 0;
            source.Play();


            StartCoroutine(FadeIn(source, volume * MasterVolume * SFXVolume, fadeTime));


            if (!loop)
            {
                Destroy(audioObject, audioClips[key].length + fadeTime);
            }


            if (!playingAudioSources.ContainsKey(key))
            {
                playingAudioSources[key] = new List<AudioSource>();
            }
            playingAudioSources[key].Add(source);


            // Return the audio source to keep a reference for StopAudio()
            return source;
        }
        else
        {
            Debug.LogWarning("Audio clip not found: " + key);
            return null;
        }
    }


    //each instance of sound can be called by at the location of an object's script using
    //"AudioManager.GetInstance().PlayAudio("SoundFileName", transform.position, 0.75f)"


    //or


    //for audio not coming from a particular location(Music or UI)
    //"AudioManager.GetInstance().PlayAudio("SoundFileName", default, 0.75f)"
   
    //the audio level is determined by the float number at the end
    //1.0f being the normal 100% volume, 0.5f being 50%, 2.0f being 200%, etc.


    public AudioSource PlayAudioFollowObject(string key, GameObject followTarget, float volume = 1.0f, bool loop = false, float maxDistance = 500f)
    {
        if (audioClips.ContainsKey(key))
        {
            GameObject audioObject = new GameObject("AudioObject_" + key);
            AudioSource source = audioObject.AddComponent<AudioSource>();


            originalVolumes.Add(source, volume);


            source.clip = audioClips[key];
            source.volume = volume * MasterVolume * SFXVolume;
            source.loop = loop;
            source.spatialBlend = 1f;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 1f;
            source.maxDistance = maxDistance;


            FollowAudioSource followScript = audioObject.AddComponent<FollowAudioSource>();
            followScript.target = followTarget.transform;


            source.Play();


            if (!loop)
            {
                Destroy(audioObject, audioClips[key].length);
            }


            if (!playingAudioSources.ContainsKey(key))
            {
                playingAudioSources[key] = new List<AudioSource>();
            }
            playingAudioSources[key].Add(source);


            // Return the audio source to keep a reference for StopAudio()
            return source;
        }
        else
        {
            Debug.LogWarning("Audio clip not found: " + key);
            return null;
        }
    }


    public AudioSource PlayAudioAtLocation(string key, Vector3 position = default, float volume = 1.0f, bool loop = false, float maxDistance = 500f)
    {
        if (audioClips.ContainsKey(key))
        {
            GameObject audioObject = new GameObject("AudioObject_" + key);
            AudioSource source = audioObject.AddComponent<AudioSource>();


            originalVolumes.Add(source, volume);


            source.clip = audioClips[key];
            source.volume = volume * MasterVolume * SFXVolume;
            source.loop = loop;


            source.spatialBlend = 1;
           
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 1f;
            source.maxDistance = maxDistance;


            if (position != Vector3.zero)
            {
                audioObject.transform.position = position;
            }


            source.Play();


            if (!loop)
            {
                Destroy(audioObject, audioClips[key].length);
            }


            if (!playingAudioSources.ContainsKey(key))
            {
                playingAudioSources[key] = new List<AudioSource>();
            }
            playingAudioSources[key].Add(source);


            // Return the audio source to keep a reference for StopAudio()
            return source;
        }
        else
        {
            Debug.LogWarning("Audio clip not found: " + key);
            return null;
        }
    }


    // This method will end all instances of a particular SFX name
    public void StopAudioOfType(string key)
    {
        if (playingAudioSources.ContainsKey(key))
        {
            List<AudioSource> sourcesToStop = new List<AudioSource>(playingAudioSources[key]);
           
            foreach (var source in sourcesToStop)
            {
                if (source != null)
                {
                    source.Stop();
                    Destroy(source.gameObject);
                    originalVolumes.Remove(source);
                }
            }
            // Clear the list after stopping and destroying the AudioSources
            playingAudioSources[key].Clear();
        }
    }
   


    //Say we have multiple instances of a single audio source, and only want to end one
    //this method allows us to end a specific instance of audio while not affecting other instances using that same audio clip
    //
    //Every function that plays audio will also return a reference to the audio currently playing,
    //simply insert that saved Audio source here to end it
    //
    //EXAMPLE:
    //AudioSource ExampleAudioSource = AudioManager.GetInstance().PlayGlobalAudio("Roar");
    //AudioManager.GetInstance().StopAudio(ExampleAudioSource);


    public void StopAudio(AudioSource source, float fadeTime = 0f)
    {
        if (source != null)
        {
            if (fadeTime > 0)
            {
                StartCoroutine(FadeOutAndDestroy(source, fadeTime));
            }
            else
            {
                source.Stop();
                Destroy(source.gameObject);
            }


            foreach (var key in playingAudioSources.Keys)
            {
                if (playingAudioSources[key].Contains(source))
                {
                    playingAudioSources[key].Remove(source);
                    break;
                }
            }


            originalVolumes.Remove(source);
        }
    }






    //VOLUME SETTERS


    public void SetMasterVolume(float newVolume){
        MasterVolume = Mathf.Clamp(newVolume, 0.0f, 1.0f);


        foreach(var audioList in playingAudioSources.Values){
            foreach(var audioSource in audioList){
                audioSource.volume = GetOriginalVolume(audioSource) * MasterVolume *SFXVolume;
            }
        }


        foreach (var audioZone in FindObjectsOfType<AudioZone>()){
            audioZone.UpdateVolumeBasedOnMasterVolume();
        }
    }


    public void SetSFXVolume(float newVolume){
        SFXVolume = Mathf.Clamp(newVolume, 0.0f, 1.0f);


        foreach(var audioList in playingAudioSources.Values){
            foreach(var audioSource in audioList){
                audioSource.volume = GetOriginalVolume(audioSource) * MasterVolume * SFXVolume;
            }
        }


        foreach (var audioZone in FindObjectsOfType<AudioZone>()){
            audioZone.UpdateVolumeBasedOnMasterVolume();
        }
    }


    public void SetMusicVolume(float newVolume){
        MusicVolume = Mathf.Clamp(newVolume, 0.0f, 1.0f);


        foreach (var audioZone in FindObjectsOfType<AudioZone>()){
            audioZone.UpdateVolumeBasedOnMasterVolume();
        }
    }


    private float GetOriginalVolume(AudioSource audioSource){
        if(originalVolumes.TryGetValue(audioSource, out float originalVolume))
        {
            return originalVolume;
        }
        else{
            Debug.LogWarning("Original volume not found for Audiosource. Using default volume.");
            return 1.0f;
        }
    }


    public float GetMasterVolume(){
        return MasterVolume;
    }


    public float GetSFXVolume()
    {
        return SFXVolume;
    }


    public float GetMusicVolume()
    {
        return MusicVolume;
    }


    private void CleanupInvalidAudioSources()
    {
        foreach (var key in new List<string>(playingAudioSources.Keys))
        {
            var sources = playingAudioSources[key];
            sources.RemoveAll(source => source == null || source.gameObject == null);
            if (sources.Count == 0)
            {
                playingAudioSources.Remove(key);
            }
        }


        var invalidEntries = originalVolumes.Where(pair => pair.Key == null || pair.Key.gameObject == null).Select(pair => pair.Key).ToList();
        foreach (var key in invalidEntries)
        {
            originalVolumes.Remove(key);
        }
    }


    //PAUSE AND UNPAUSE ALL AUDIO, this may need editing if interactions are weird with pause menu audio later on, we'll see
    public void PauseAllAudio()
    {
        foreach (var audioList in playingAudioSources.Values)
        {
            foreach (var audioSource in audioList)
            {
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Pause();
                }
            }
        }
    }


    public void ResumeAllAudio()
    {
        foreach (var audioList in playingAudioSources.Values)
        {
            foreach (var audioSource in audioList)
            {
                if (audioSource != null && !audioSource.isPlaying)
                {
                    audioSource.UnPause();
                }
            }
        }
    }




    //COROUTINES


    IEnumerator FadeIn(AudioSource audioSource, float targetVolume, float fadeTime){
        float startVolume = 0f;
        float time = 0;


        while(time<fadeTime){
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, time/fadeTime);
            time += Time.deltaTime;
            yield return null;
        }


        audioSource.volume = targetVolume;
    }


    IEnumerator FadeOutAndDestroy(AudioSource audioSource, float fadeTime)
    {
        float currentVolume = audioSource.volume;
        float time = 0;


        while (time < fadeTime)
        {
            audioSource.volume = Mathf.Lerp(currentVolume, 0f, time / fadeTime);
            time += Time.deltaTime;
            yield return null;
        }


        audioSource.volume = 0f;
        audioSource.Stop();
        Destroy(audioSource.gameObject);
    }
}
