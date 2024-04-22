using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitivityAdjuster : MonoBehaviour
{
    public void AdjustSensitivity(float newSensitivityMultiplier){
        SettingsManager.GetInstance().SetNewSensitivity(newSensitivityMultiplier);
    }
}
