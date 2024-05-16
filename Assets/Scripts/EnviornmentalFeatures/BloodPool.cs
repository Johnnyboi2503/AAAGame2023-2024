using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPool : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float gainBloodAmount;
    [SerializeField] float gainTickRate;
    [SerializeField] bool canOverfeed;

    [Space]
    [Header("Audio")]
    [SerializeField] private float bloodPoolAudioVolume = 0.65f;
    [SerializeField] private float bloodPoolAudioDistanceRange = 15f;

    float gainTickTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.GetInstance().PlayAudioAtLocation("BloodPool_SFX", transform.position, bloodPoolAudioVolume, true, bloodPoolAudioDistanceRange);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.GetInstance().StopAudioOfType("BloodPool_SFX");
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.TryGetComponent(out BloodThirst bloodThirst)) {
            gainTickTimer -= Time.deltaTime;
            if(gainTickTimer <= 0) {
                bloodThirst.GainBlood(gainBloodAmount, canOverfeed);
                gainTickTimer = gainTickRate;
            }
        }
    }
}
