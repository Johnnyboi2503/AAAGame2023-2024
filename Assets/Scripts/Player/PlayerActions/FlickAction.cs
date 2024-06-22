using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickAction : PlayerAction
{
    [Header("References")]
    [SerializeField] GameObject swordObject;

    // Variables
    [Header("Movement Variables")]
    [SerializeField] float jumpForce; // Jump force added when flicking
    [SerializeField] float horizontalForce; // The horizontal force added when flicking
    [SerializeField] float initalSpeedScale; // How much the player impacts the speed, measured in percent (i.e. value of 0.1 == 10% of player speed is factored)
    [SerializeField] float horizontalSpeedLimit; // The max speed AFTER inital velocity + speed + bonus CALCULATION, applied just for the horizontal force
    [SerializeField] float jumpSpeedLimit; // The max speed AFTER inital velocity + speed + bonus CALCULATION, applied just for the jump force
    [SerializeField] float stunnedEnemyBonus; // amount of speed added when flicking off a stunned enemy, they give a bonus to movement

    [Header("Boosted Variables")]
    [SerializeField] float boostedJumpForce;
    [SerializeField] float boostedHorizontalForce;
    [SerializeField] float boostedInitalSpeedScale;
    [SerializeField] float boostedHorizontalSpeedLimit;
    [SerializeField] float boostedJumpSpeedLimit;
    [SerializeField] float boostedStunnedEnemyBonus;
    [Space]
    [Range(0.0f, 1f)]
    [SerializeField] private float flickTrauma;

    [Header("References")]
    [SerializeField] SwordMovement swordMovement;

    // References
    CameraFov cameraFov;
    Rigidbody rb;
    JumpAction jumpAction;
    MovementModification movementModification;

    // Action Relevant variables
    FlickEnemyStabable flickEnemy;
    FlickEnviornmentStabable flickEnviornment;
    bool sticking = false;
    Vector3 stickPos;
    Vector3 initalVelocity;
    Vector3 directionInput;

    private void Start() {
        // Getting references
        cameraFov = FindObjectOfType<CameraFov>();
        rb = GetComponent<Rigidbody>();
        jumpAction = GetComponent<JumpAction>();
        movementModification = GetComponentInChildren<MovementModification>();
    }

    private void FixedUpdate() {
        if(sticking) {
            StickUpdate();
        }
    }
    private void OnEnable()
    {
        OnStartAction.AddListener(() => cameraFov.IncreaseTrauma(flickTrauma));
    }
    private void OnDisable()
    {
        OnStartAction.RemoveListener(() => cameraFov.IncreaseTrauma(flickTrauma));
    }


    // Sticking to the object
    public void Stick(FlickEnemyStabable _flickEnemy = null, FlickEnviornmentStabable _flickEnviornment = null) {
        // Setting variables
        sticking = true;
        flickEnemy = _flickEnemy;
        flickEnviornment = _flickEnviornment;

        // Setting inital values
        stickPos = transform.position;
        initalVelocity = rb.velocity;
        rb.velocity = Vector3.zero;

        OnStartAction.Invoke();
    }

    // Input gotten from player
    public void HorizontalInput(Vector3 direction) {
        directionInput = direction.normalized;
    }

    public void FlickOff() {
        sticking = false;

        // Boosting calcs
        float currentJump = movementModification.GetBoost(jumpForce, boostedJumpForce, true);
        float currentHorizontal = movementModification.GetBoost(horizontalForce, boostedHorizontalForce, true);
        float currentPlayerSpeed = initalVelocity.magnitude * movementModification.GetBoost(initalSpeedScale, boostedInitalSpeedScale, false);
        float currentHorizontalLimit = movementModification.GetBoost(horizontalSpeedLimit, boostedHorizontalSpeedLimit, false);
        float currentJumpLimit = movementModification.GetBoost(jumpSpeedLimit, boostedJumpSpeedLimit, false);

        // Applying player speed
        currentHorizontal = currentHorizontal + currentPlayerSpeed;
        currentJump = currentJump + currentPlayerSpeed;

        // Adding bonus movement based on stunned enemy
        if (flickEnemy != null) {
            if (flickEnemy.stunned) {
                float currentBonus = movementModification.GetBoost(stunnedEnemyBonus, boostedStunnedEnemyBonus, true);
                currentHorizontal += currentBonus;
                currentJump += currentBonus;
            }

            // Killing enemy before movement
            flickEnemy.Die();
        }

        // For flick enviornment
        if(flickEnviornment != null) {
            directionInput = (flickEnviornment.horzDirPosition.position - transform.position).normalized;
            flickEnviornment.MakePassable(0.5f); // Turning off the collider for 0.5 sec
        }

        // Setting Limits
        Vector3 horizontalVelocity = Mathf.Min(currentHorizontalLimit, currentHorizontal) * directionInput;
        currentJump = Mathf.Min(currentJumpLimit, currentJump);

        // Applying movements
        jumpAction.Jump(currentJump);
        rb.velocity += horizontalVelocity;

        // Ending Action
        EndAction();
    }
    private void StickUpdate() {
        // Sticking objects (sword may be temp)
        rb.position = stickPos;
        rb.velocity = Vector3.zero;


        // Setting forward direction and sword pos
        Vector3 horzDir = Vector3.forward;
        if (flickEnemy != null) {
            horzDir = flickEnemy.GetComponent<Collider>().ClosestPoint(transform.position) - transform.position;
            swordMovement.UpdateFlick(rb.position, flickEnemy.GetComponent<Collider>());
        }
        else if (flickEnviornment != null) {
            horzDir = flickEnviornment.GetComponent<Collider>().ClosestPoint(transform.position) - transform.position;
            swordMovement.UpdateFlick(rb.position, flickEnviornment.GetComponent<Collider>());
        }
        horzDir.y = 0;
        transform.forward = horzDir.normalized;
    }

    public override void EndAction() {
        if(sticking) {
            sticking = false;
        }
        swordMovement.EndAttackPosition();
        OnEndAction.Invoke();
    }
}
