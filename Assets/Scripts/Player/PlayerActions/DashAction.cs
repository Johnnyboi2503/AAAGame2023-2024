using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DashAction : PlayerAction
{
    [Header("Movement")]
    [SerializeField] float dashSpeed; // The speed player will add onto current speed
    [SerializeField] float dashDuration; // How long the dash lasts
    [SerializeField] float dashCooldown; // Cooldown between consecutive dashes
    [SerializeField] float endDashSpeedBonus; // Speed at the end of the dash
    [SerializeField] float initalSpeedScale; // How much the player impacts the speed, measured in percent (i.e. value of 0.1 == 10% of player speed is factored)
    [SerializeField] float speedLimit; // The max speed AFTER inital velocity + speed + bonus speed CALCULATION (so this limit applies for both the exit speed and the action itself) 
    [SerializeField] float durationMin; // The minimum dash duration AFTER all the calculations

    [Header("Boosted Movement")]
    [SerializeField] float boostedDashSpeed;
    [SerializeField] float boostedDashDuration;
    [SerializeField] float boostedDashCooldown;
    [SerializeField] float boostedEndDashSpeedBonus;
    [SerializeField] float boostedInitalSpeedScale;
    [SerializeField] float boostsedSpeedLimit;
    [SerializeField] float boostedDurationMin;

    [Space]
    [SerializeField] private float dashAudioVolume = 1f;
    [Range(0.0f, 1f)]
    [SerializeField] private float dashTrauma;

    bool dashAvailable = true;
    float dashCdTimer;// Time before you can dash again
    public float timer = 0; // Used for starting slash/stab dash
    Vector3 playerInitalVelocity;

    // References
    Rigidbody rb;
    MovementModification movementModification;
    PlayerPositionCheck playerPositionCheck;
    DashMovement dashMovement;
    CameraFov cameraFov;
    // Temp for visual clarity
    Renderer render;
    Color holder;

    // Start is called before the first frame update
    void Start()
    {
        // Getting components
        rb = GetComponent<Rigidbody>();
        movementModification = GetComponentInChildren<MovementModification>();
        playerPositionCheck = GetComponentInChildren<PlayerPositionCheck>();
        dashMovement = new DashMovement(transform, rb);
        dashMovement.OnDashEnd.AddListener(EndAction);
        cameraFov = FindObjectOfType<CameraFov>();
        // Temp for visual clarity
        render = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        OnStartAction.AddListener(() => cameraFov.IncreaseTrauma(dashTrauma));
    }
    private void OnDisable()
    {
        OnStartAction.RemoveListener(() => cameraFov.IncreaseTrauma(dashTrauma));
    }

    // Update is called once per frame
    void Update()
    {
        // if dash is still on cd, count down the timer 
        if (dashCdTimer > 0) { 
            dashCdTimer -= Time.deltaTime;
        }

        // Ground Check
        if (playerPositionCheck.CheckOnGround()) {
            ResetDash();
        }

        if(dashMovement.isDashing) {
            timer += Time.deltaTime;
        }
    }
    private void FixedUpdate() {
        dashMovement.UpdateDashing();
    }
    // Input for this actio
    public void DashInput(Vector3 direction)
    {
        // Temp
        holder = render.material.color;
        render.material.color = Color.blue;
        playerInitalVelocity = rb.velocity;

        timer = 0;
        dashAvailable = false; // Using up the dash

        // Calculating boosts (all boosts are calculated as a linear interpolation between normal and boost amount given a percentage)
        dashCdTimer = movementModification.GetBoost(dashCooldown, boostedDashCooldown, true);
        float currentDashSpeed = movementModification.GetBoost(dashSpeed, boostedDashSpeed, true);
        float currentDashDuration = movementModification.GetBoost(dashDuration, boostedDashDuration, true);
        float currentEndDashSpeedBonus = movementModification.GetBoost(endDashSpeedBonus, boostedEndDashSpeedBonus, true);
        float currentVelocity = rb.velocity.magnitude * movementModification.GetBoost(initalSpeedScale, boostedInitalSpeedScale, false);

        // Limiting Speed
        float currentMaxSpeed = movementModification.GetBoost(speedLimit, boostsedSpeedLimit, false);
        float currentMinDuration = movementModification.GetBoost(durationMin, boostedDurationMin, false);

        // Applying limits
        float appliedDashSpeed = Mathf.Min(currentMaxSpeed, currentVelocity + currentDashSpeed);
        float appliedDashDuration = Mathf.Max(currentMinDuration, currentDashDuration);
        float appliedExitSpeed = Mathf.Min(currentMaxSpeed, appliedDashSpeed + currentEndDashSpeedBonus);

        dashMovement.Dash(appliedDashSpeed, appliedDashDuration, direction, appliedExitSpeed);

        // Play Dash Audio
        AudioManager.GetInstance().PlayAudioFollowObject("Dash_SFX", gameObject, dashAudioVolume);

        OnStartAction.Invoke();
    }

    // Resets the dash allowing player to dash again
    public void ResetDash()
    {
        dashCdTimer = 0;
        dashAvailable = true;
    }

    public void ConsumeDash() {
        dashAvailable = false;
    }

    // Checking if player can perform a dash
    public bool CanPerformDash() {
        return dashCdTimer <= 0 && dashAvailable && !dashMovement.isDashing;
    }

    // This is used for comination inputs to interupt the dash and start the actual action
    public void InteruptDash() {
        EndAction();
        rb.velocity = playerInitalVelocity;
    }

    // End this action
    public override void EndAction() {
        render.material.color = holder;

        //Ending dash
        if (dashMovement.isDashing) {
            dashMovement.InteruptDash();
        }
        OnEndAction.Invoke();
    }

  
}
