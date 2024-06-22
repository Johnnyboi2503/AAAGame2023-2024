using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StabDashAction : PlayerAction
{
    [Header("References")]
    [SerializeField] DashCollider dashCollider;
    CameraFov cameraFov;
    [Header("Dash Variables")]
    [SerializeField] float dashSpeed; // The speed on top of the player velocity
    [SerializeField] float dashDuration; // How long the dash takes
    [SerializeField] float endDashSpeedBonus; // How fast the player is going at the end of the dash
    [SerializeField] float initalSpeedScale;  // How much the player impacts the speed, measured in percent (i.e. value of 0.1 == 10% of player speed is factored)
    [SerializeField] float speedLimit; // The max speed AFTER inital velocity + speed + bonus speed CALCULATION (so this limit applies for both the exit speed and the action itself) 
    [SerializeField] float durationMin; // The minimum dash duration AFTER all the calculations

    [Header("Boosted Variables")]
    [SerializeField] float boostedDashSpeed;
    [SerializeField] float boostedDashDuration;
    [SerializeField] float boostedEndDashSpeedBonus;
    [SerializeField] float boostedInitalSpeedScale;
    [SerializeField] float boostsedSpeedLimit;
    [SerializeField] float boostedDurationMin;
    
    [Header("Other Variables")]
    [SerializeField] float bloodGained; // How much blood is gained when striking something stabable
    [Range(0.0f, 1f)]
    [SerializeField] float stabDashTrauma;
    // Temp color change
    Renderer render;
    Color holder;

    // Components
    Rigidbody rb;
    DashMovement dashMovement;
    DashAction dashAction;
    StabContact stabContact;
    MovementModification movementModification;


    //Variables
    bool isDashing = false;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        render = GetComponent<Renderer>();
        dashMovement = new DashMovement(transform, GetComponent<Rigidbody>());
        stabContact = GetComponentInChildren<StabContact>();
        movementModification = GetComponentInChildren<MovementModification>();
        dashAction = GetComponent<DashAction>();
        cameraFov = FindObjectOfType<CameraFov>();
        // Setting events
        dashMovement.OnDashEnd.AddListener(EndAction);

        // Temp holder
        holder = render.material.color;
    }
    private void OnEnable()
    {
        OnStartAction.AddListener(() => cameraFov.IncreaseTrauma(stabDashTrauma));
    }
    private void OnDisable()
    {
        OnStartAction.RemoveListener(() => cameraFov.IncreaseTrauma(stabDashTrauma));
    }


    private void FixedUpdate() {
        dashMovement.UpdateDashing();
    }

    public void StabDashInput(Vector3 direction) {
        if(!isDashing) {
            isDashing = true;
            render.material.color = Color.red;
            dashAction.ConsumeDash();
            stabContact.ActivateContactEvent(dashCollider.OnContact, bloodGained);

            // Calculating boosted variables
            float currentDashSpeed = movementModification.GetBoost(dashSpeed, boostedDashSpeed, true);
            float currentDashDuration = movementModification.GetBoost(dashDuration, boostedDashDuration, true);
            float currentEndDashSpeedBonus = movementModification.GetBoost(endDashSpeedBonus, boostedEndDashSpeedBonus, true);
            float currentVelocity = rb.velocity.magnitude * movementModification.GetBoost(initalSpeedScale, boostedInitalSpeedScale, false);

            // Limiting Speed
            float currentMaxSpeed = movementModification.GetBoost(speedLimit, boostsedSpeedLimit, false);
            float currentMinDuration = movementModification.GetBoost(durationMin, boostedDurationMin, false);

            float appliedDashSpeed = Mathf.Min(currentMaxSpeed, currentVelocity + currentDashSpeed);
            float appliedDashDuration = Mathf.Max(currentMinDuration, currentDashDuration);
            float appliedExitSpeed = Mathf.Min(currentMaxSpeed, appliedDashSpeed + currentEndDashSpeedBonus);

            dashMovement.Dash(appliedDashSpeed, appliedDashDuration, direction, appliedExitSpeed);

            OnStartAction.Invoke();
        }
    }

    public override void EndAction() {
        isDashing = false;
        render.material.color = holder;
        if(dashMovement.isDashing) {
            dashMovement.InteruptDash();
        }
        stabContact.EndContactEvent();
        OnEndAction.Invoke();
    }
}
