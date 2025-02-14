using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAction : PlayerAction
{
    // References
    Rigidbody rb;
    PlayerPositionCheck positionCheck;
    MovementModification movementModification;

    [Header("Jump Variables")]
    [SerializeField] float initalSpeed; // Inital Speed that is applied reguardless of how long pressed
    [SerializeField] float dampWindow; // how long the player has to release the button to dapen the jump
    [Range(0, 1)]
    [SerializeField] float damping; // how much the jump is decreased by (0.2 will decrease the jump height by 20% when jump button is released)

    [Header("Boosted Jump")]
    [SerializeField] float boostedInitalSpeed;
    [SerializeField] float boostedDampWindow; 
    [Range(0, 1)]
    [SerializeField] float boostedDamping;

    [Space]
    [SerializeField] private float jumpAudioVolume = 1f;

    float jumpTimer = 0f;
    bool jumping = false;
    bool canAirJump = false;
    bool grounded = false;

    private void Start() {
        // Getting references
        rb = GetComponent<Rigidbody>();
        positionCheck = GetComponentInChildren<PlayerPositionCheck>();
        movementModification = GetComponentInChildren<MovementModification>();
    }

    private void FixedUpdate() {
        if(jumping) {
            jumpTimer += Time.deltaTime;
            if (jumpTimer > movementModification.GetBoost(dampWindow, boostedDampWindow, false)) {
                EndAction();
            }
        }
    }

    private void Update() {
        grounded = positionCheck.CheckOnGround();
        if(grounded) {
            canAirJump = false;
        }
    }

    // Run for player input jump
    public void JumpInputPressed() {
        // Performing jump
        if (CanPerformJump()) {
            if(canAirJump) {
                canAirJump = false;
            }
            JumpStart();
        }
    }

    // Starts the actual jump
    public void JumpStart() {
        Jump(movementModification.GetBoost(initalSpeed, boostedInitalSpeed, true));
        jumping = true;

        // Play jump audio
        string soundEffect = Random.Range(0, 2) == 0 ? "Jump_SFX" : "Jump2_SFX";
        AudioManager.GetInstance().PlayAudioFollowObject(soundEffect, gameObject, jumpAudioVolume);

        OnStartAction.Invoke();
    }

    public void JumpInputRelease() {
        // Dampeneing the speed when below the boosted amount
        if(rb.velocity.y <= movementModification.GetBoost(initalSpeed, boostedInitalSpeed, true) && rb.velocity.y > 0) {
            rb.velocity -= rb.velocity * movementModification.GetBoost(damping, boostedDamping, true);
        }
        EndAction();
    }

    public void Jump(float jumpForce) {
        if (rb.velocity.y < 0) {
            rb.velocity = transform.up * jumpForce;
        }
        else {
            rb.velocity += transform.up * jumpForce;
        }
    }
    public void GiveAirJump() {
        canAirJump = true;
    }

    public bool CanPerformJump() {
        return (grounded || canAirJump) && !positionCheck.CheckOnSlope();
    }

    public override void EndAction() {
        jumping = false;
        jumpTimer = 0;
        OnEndAction.Invoke();
    }
}