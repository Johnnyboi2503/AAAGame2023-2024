using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class StabAction : PlayerAction {
    [Header("References")]
    [SerializeField] SwordMovement swordMovement; // Do not adjust
    CameraFov cameraFov;

    [Header("Other Variables")]
    [SerializeField] float bloodGainAmount; // Amount of blood gained from striking something 
    [SerializeField] float attackDuration; // How long the sword swing lasts, temperary implemention for testing

    [Space]
    [SerializeField] float stabAudioVolume = 1f;
    [Range(0.0f, 1f)]
    [SerializeField] float stabTrauma;
    // Movement Compoenents
    StabContact stabContact;
   

    // Variables
    public bool isStabbing = false;
    public float timer = 0; // used for the stab dash

    // Start is called before the first frame update
    void Start()
    {
        // Getting components
        stabContact = GetComponentInChildren<StabContact>();
        cameraFov = FindObjectOfType<CameraFov>();
    }

    private void Update() {
        if(isStabbing) {
            timer += Time.deltaTime;
        }
    }
    private void OnEnable()
    {
        OnStartAction.AddListener(() => cameraFov.IncreaseTrauma(stabTrauma));
    }
    private void OnDisable()
    {
        OnStartAction.RemoveListener(() => cameraFov.IncreaseTrauma(stabTrauma));
    }

    public void StabInput()
    {
        //Animation Stuff (to be implemented later)
        isStabbing = true;
        timer = 0;

        // Initalizing stab contact
        stabContact.ActivateContactEvent(swordMovement.OnContact, bloodGainAmount);
        
        // Demon sword variables
        swordMovement.OnEndAction.AddListener(EndOfStabAnimation);
        swordMovement.StabPosition(attackDuration);

        // Play Stab Audio
        AudioManager.GetInstance().PlayAudioFollowObject("Stab_SFX", gameObject, stabAudioVolume);

        OnStartAction.Invoke();
    }


    // This occurs at the end of the stab animation
    private void EndOfStabAnimation() {
        swordMovement.OnEndAction.RemoveListener(EndOfStabAnimation);
        EndAction();
    }

    // Checking if you can perform stab
    public bool CanPerformStab() {
        return !isStabbing;
    }
    public override void EndAction() {
        isStabbing = false;
        if (swordMovement.isAttacking) {
            swordMovement.EndAttackPosition();
        }
        stabContact.EndContactEvent();
        OnEndAction.Invoke();
    }
}
