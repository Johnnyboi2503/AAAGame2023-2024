using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BloodThirst : MonoBehaviour
{
    [Header("Blood Variables")]
    [SerializeField] public float bloodThirstThreshold; // Threshold for blood thirst
    [SerializeField] public float maxBlood; // Max amount of blood before overfed
    [SerializeField] public float maxBloodForOverfed; // Max amount of blood for overfed
    [SerializeField] public float currentBlood; // Current blood amount

    [SerializeField] float bloodDrainRate; // Blood drain rate
    [SerializeField] float overfedDrainRate; // Blood drain rate when overfed

    [SerializeField] bool canDrainBlood = true;
    [SerializeField] float bloodthirstBarAudioVolume = 1f;

    [Header("Player Damaged Variables")]
    [SerializeField] float stunDuration; // amount of stun when hit
    [SerializeField] float knockbackStrength; // knockback from enemy

    [Header("Other Variables")]
    [SerializeField] PlayerKillable playerKillable;
    [SerializeField] float playerHealthDrainRate; // How much are you draining from the player

    [Header("Events")]
    public UnityEvent OnBloodChange = new UnityEvent(); // Sends signal update to the UI

    [Header("Camera Shake")] 
    [SerializeField] private float _shakeDuration = 0.5f;
    [SerializeField] private float _shakeAmplitude = 1.5f;
    [SerializeField] private float _shakeFrequency = 1f;
    private CameraShake _cameraShake;
    
    // Components
    MovementModification movementModification;
    
    // Other Variables
    bool isDraining = false; // If the sword is currently draining

    // Start is called before the first frame update
    void Start()
    {
        //Initalizing values
        currentBlood = maxBlood;

        if(gameObject.TryGetComponent(out movementModification))
        {
            Debug.Log("Movement modification not found");
        }

        playerKillable.OnDie.AddListener(OnDeath);
        
        // Cache CameraShake
        _cameraShake = FindObjectOfType<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        DrainBlood();

        if(currentBlood < bloodThirstThreshold)
        {
            isDraining = true;
        }
        else
        {
            isDraining = false;
        }

        if(isDraining)
        {
            playerKillable.TakeDamage(playerHealthDrainRate * Time.deltaTime);
        }

        if (movementModification != null)
        {
            ApplyMovementModification();
        }
    }
    private void DrainBlood()
    {
        if (canDrainBlood) {
            // Draining blood based on overfed and limiting limit to 0
            if (currentBlood < maxBlood) {
                currentBlood = Mathf.Max(currentBlood - (bloodDrainRate * Time.deltaTime), 0);
            }
            else {
                currentBlood -= overfedDrainRate * Time.deltaTime;
            }
            OnBloodChange.Invoke();
        }
    }
    private void ApplyMovementModification()
    {
        // Setting boost based on thresholds but not letting it drop below zero
        movementModification.SetBoost(Mathf.Max(Mathf.InverseLerp(maxBlood, maxBloodForOverfed, currentBlood), 0));
    }

    public void GainBlood(float amount, bool canOverFeed)
    {
        // Adding blood based on if you can overfeed and limiting it based on max
        if (canOverFeed) {
            currentBlood = Mathf.Min(currentBlood+amount, maxBloodForOverfed);

            // If the player is over max blood due to overfed, play the blood thirst bar audio
            if (currentBlood > maxBlood)
            {
                AudioManager.GetInstance().PlayGlobalAudio("BloodthirstBar_SFX", bloodthirstBarAudioVolume);
            }
        }
        else {
            if(currentBlood < maxBlood) {
                currentBlood = Mathf.Min(currentBlood + amount, maxBlood);
            }
        }
        OnBloodChange.Invoke();
    }

    public void LoseBlood(float amount, GameObject attacker)
    {
        PlayerStun stun = this.gameObject.GetComponentInChildren<PlayerStun>();

        if (!stun.IsStunned()) {
            // stuns the player
            stun.Stun(stunDuration);


            // Getting direciton
            Vector3 moveDirection = this.gameObject.transform.position - attacker.transform.position;

            // Getting and normalizing horizontal direction
            moveDirection.y = 0;
            moveDirection = moveDirection.normalized;

            // Setting vertical components and applying normalized value
            moveDirection.y = 1; // Giving vertical component
            this.gameObject.GetComponentInParent<Rigidbody>().velocity = moveDirection.normalized * knockbackStrength;


            if (_cameraShake != null) {
                _cameraShake.ShakeCamera(_shakeDuration, _shakeAmplitude, _shakeFrequency);
            }

            PayBlood(amount);
        }
    }

    public void PayBlood(float amount) {
        currentBlood -= amount;
        currentBlood = Mathf.Clamp(currentBlood, 0, maxBlood);

        OnBloodChange.Invoke();
    }

    public void OnDeath()
    {
        // Stop All Audio
        AudioManager.GetInstance().StopAudioOfType("BloodthirstBar_SFX");
    }
}
