using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeAdjuster : MonoBehaviour
{
    public void AdjustVolume(float newVolume) {
        AudioManager.GetInstance().SetMasterVolume(newVolume);
    }
}
