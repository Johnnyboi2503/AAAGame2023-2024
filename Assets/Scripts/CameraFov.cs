using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraFov : MonoBehaviour
{
    public float defaultFOV;
    public float maxFovForChange;//The max allowed  fov 
    public float traumaDecayRate;
    public float smoothFOV; 

    private float trauma = 0f;
    private float fovVelocity = 0f;

    CinemachineFreeLook mainCamera;

    private void Start()
    {
        mainCamera = GetComponent<CinemachineFreeLook>();
    }
    void Update()
    {


        float fovAdjustment = maxFovForChange * Mathf.Pow(trauma, 2);
        float desiredFOV = defaultFOV + fovAdjustment;

        float currentFOV = mainCamera.m_Lens.FieldOfView;
        float newFOV = Mathf.SmoothDamp(currentFOV, desiredFOV, ref fovVelocity, smoothFOV);

        mainCamera.m_Lens.FieldOfView = newFOV;

        if (trauma > 0)
        {
            trauma -= traumaDecayRate * Time.deltaTime;
            trauma = Mathf.Clamp01(trauma);
        }

    }

    public void IncreaseTrauma(float amount)
    {
        trauma += amount;
        trauma = Mathf.Clamp01(trauma);  // trauma stays within [0,1]
    }
}

