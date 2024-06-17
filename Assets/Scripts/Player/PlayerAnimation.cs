using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerAnimation : MonoBehaviour
{
    // references
    BasicMovementAction groundedCheck;
    DashAction dashAction;
    SlashAction slashAction;
    StabAction stabAction;

    // components
    Animator anim;
    Rigidbody rb;
    ParentConstraint swordPC;
    AimConstraint ac;

    // references demon sword for parent constraint & player's hand for aim constraint
    [SerializeField] GameObject demonSword;
    [SerializeField] GameObject playerArm;

    private Vector3 origPos;
    private Quaternion origRot;

    // Start is called before the first frame update
    void Start() {
        // getting components
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
        dashAction = GetComponentInParent<DashAction>();
        slashAction = GetComponentInParent<SlashAction>();
        stabAction = GetComponentInParent<StabAction>();
        swordPC = demonSword.GetComponent<ParentConstraint>();
        ac = playerArm.GetComponent<AimConstraint>();

        dashAction.OnStartAction.AddListener(DashAnimation);
        dashAction.OnEndAction.AddListener(EndDashAnimation);
        slashAction.OnStartAction.AddListener(SlashAnimation);
        stabAction.OnStartAction.AddListener(StabAnimation);

        origPos = demonSword.transform.localPosition;
        origRot = demonSword.transform.localRotation;
    }

    // Update is called once per frame
    void Update() {
        BasicAnimation();
    }

    void BasicAnimation()
    {
        // walking animation works based on pos/neg x velocity
        if (rb.velocity.x != 0) {
            anim.SetBool("isWalking", true);
        }
        else {
            anim.SetBool("isWalking", false);
        }

        // jumping animation works based on pos/neg y velocity
        if (rb.velocity.y > 0.0001) {
            anim.SetBool("isJumping", true);
            Debug.Log(rb.velocity.y);
        }
        else {
            anim.SetBool("isJumping", false);
        }
    }

    public void DashAnimation() {
        anim.SetBool("isDashing", true);
    }

    public void EndDashAnimation() {
        anim.SetBool("isDashing", false);
    }

    public void SlashAnimation() {
        anim.SetTrigger("WeaponGrab");
        anim.SetTrigger("SlashAttack");
    }

    public void StabAnimation() {
        anim.SetTrigger("WeaponGrab");
        anim.SetTrigger("StabAttack");
    }

    void StartSwordEvent() {
        swordPC.constraintActive = true;
    }

    void EndSwordEvent() {
        swordPC.constraintActive = false;
        demonSword.transform.localPosition = origPos;
        demonSword.transform.localRotation = origRot;
    }

    void StartWeaponGrabEvent() { 
        ac.constraintActive = true;
    }

    void EndWeaponGrabEvent() {
        swordPC.constraintActive = true;
        ac.constraintActive = false;
    }
}
