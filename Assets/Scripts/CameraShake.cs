using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;


public class CameraShake : MonoBehaviour
{
    private bool _isShaking = false;
    private float _shakeDuration = 0.5f;
    private float _timeElapsed = 0f;

    private CinemachineFreeLook cinemachineCam;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    ///=////////////////////////////////////////////////////////////////////////////////
    /// 
    private void Awake()
    {
        cinemachineCam = GetComponentInChildren<CinemachineFreeLook>();
        _cinemachineBasicMultiChannelPerlin = cinemachineCam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    ///=////////////////////////////////////////////////////////////////////////////////
    ///
    private void Update()
    {
       OnUpdateCameraShaking();
    }

    ///=////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnUpdateCameraShaking()
    {
        if (_isShaking)
        {
            if (_timeElapsed < _shakeDuration)
            {
                _timeElapsed += Time.deltaTime;
            }
            else
            {
                _isShaking = false;
                
                // Turn of shaking
                _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                _cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0f;
            }
        }
    }
    
    ///=////////////////////////////////////////////////////////////////////////////////
    ///
    public void ShakeCamera(float duration = 0.5f, float amplitudeGain = 1f, float frequencyGain = 1f)
    {
        _isShaking = true;
        
        _shakeDuration = duration;
        _timeElapsed = 0f;

        // Turn on shaking
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitudeGain;
        _cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequencyGain;
    }
}
