using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitivityAdjuster : MonoBehaviour
{
    private PlayerInput playerInput; 
    [SerializeField]private float initialMKXSensitivity = 300;
    [SerializeField]private float initialMKYSensitivity = 2;

    void Start() {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    public void AdjustOverallSensitivity(float sensitivityMultiplier) {
        float newMKXSensitivity = initialMKXSensitivity * sensitivityMultiplier;
        float newMKYSensitivity = initialMKYSensitivity * sensitivityMultiplier;

        playerInput.SetMKSensitivity(newMKXSensitivity, newMKYSensitivity);
    }
}
