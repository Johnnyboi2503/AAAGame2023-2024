using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScapeAudio : MonoBehaviour
{
    enum SoundScapeType
    {
        SoundScape1,
        SoundScape2,
    }
    [SerializeField] private SoundScapeType soundScapeType;
    [SerializeField] private float soundScapeVolume = 0.5f;

    void Start(){
        switch(soundScapeType){
            case SoundScapeType.SoundScape1:
                AudioManager.GetInstance().PlayAudioFollowObject("SoundScapeAudio", gameObject, soundScapeVolume, true, 200f);
                break;
            case SoundScapeType.SoundScape2:
                AudioManager.GetInstance().PlayAudioFollowObject("SoundScapeAudio2", gameObject, soundScapeVolume, true, 200f);
                break;
        }
    }
}
