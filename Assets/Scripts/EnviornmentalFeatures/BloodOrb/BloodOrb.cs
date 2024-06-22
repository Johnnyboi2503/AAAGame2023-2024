using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodOrb : VacuumableObject
{
    [Header("Blood Orb")]
    [SerializeField] public float gainBloodAmount;
    [SerializeField] private float interactionAudioVolume = 0.75f;


    ///-//////////////////////////////////////////////////////////////////////
    ///
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BloodThirst bloodThirst))
        {
            // Play audio if player collects blood orb
            AudioManager.GetInstance().PlayAudioAtLocation("TerrainInteration_SFX", transform.position, interactionAudioVolume);

            bloodThirst.GainBlood(gainBloodAmount, true);
            SpawnBloodEffect();
            Destroy(gameObject);
        }
    }

    private void SpawnBloodEffect() {
        EffectsController effect = FindObjectOfType<EffectsController>();
        if (effect != null) {
            effect.CircleBurst(transform.position);
        }
    }

    ///-//////////////////////////////////////////////////////////////////////
    ///
    protected override void InVacuumUpdate()
    {
        base.InVacuumUpdate();
    }
}