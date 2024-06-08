using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeAdjuster : MonoBehaviour
{
    public void AdjustVolume(float newVolume) {
        AudioManager.GetInstance().SetMasterVolume(newVolume);
    }

    public void AdjustMusicVolume(float newVolume) {
        AudioManager.GetInstance().SetMusicVolume(newVolume);
    }

    public void AdjustSFXVolume(float newVolume) {
        AudioManager.GetInstance().SetSFXVolume(newVolume);
    }
}
