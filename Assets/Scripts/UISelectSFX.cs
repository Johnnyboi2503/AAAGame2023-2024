using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectSFX : MonoBehaviour
{
    [SerializeField]float uiSelectSFXVolume = 1.0f;
    public void PlaySelectSFX()
    {
        AudioManager.GetInstance().PlayGlobalAudio("UISelectSFX", uiSelectSFXVolume);
    }
}
