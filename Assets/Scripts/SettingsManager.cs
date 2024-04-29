using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private PlayerInput playerInput;
    static SettingsManager instance;
    public static SettingsManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<SettingsManager>();

            if (instance == null)
            {
                GameObject obj = new GameObject("SettingsManager");
                instance = obj.AddComponent<SettingsManager>();
            }
        }
        return instance;
    }
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
    }

    public float initialMKXSensitivity { get; private set; } = 300f;  // Default X 
    public float initialMKYSensitivity { get; private set; } = 2f; // Default Y
    public float sensitivityMultiplier = 1f; //default sensitivity value

    void Start() {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    public void SetNewSensitivity(float newSensitivityMultiplier){
        sensitivityMultiplier = newSensitivityMultiplier;
    }

    void AdjustOverallSensitivity(float newSensitivityMultiplier) {
        float newMKXSensitivity = initialMKXSensitivity * newSensitivityMultiplier;
        float newMKYSensitivity = initialMKYSensitivity * newSensitivityMultiplier;
        
        playerInput = FindObjectOfType<PlayerInput>();
        playerInput.SetMKSensitivity(newMKXSensitivity, newMKYSensitivity);
    }

    void Update(){
        AdjustOverallSensitivity(sensitivityMultiplier);
    }
}
