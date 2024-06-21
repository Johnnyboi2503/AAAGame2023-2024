using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMovementTemp : SwordMovement {
    [Header("Player Positions")]
    [SerializeField] Transform followTarget;
    [SerializeField] Transform stabTransform;
    [SerializeField] Transform slashTransform;
    [SerializeField] Transform downwardStabTransform;
    [SerializeField] Transform dashAttackTransform;


    [Range(0f, 1f)]
    [SerializeField] float followingSpeed;

    // Internal Values
    private bool isFollowing = true;
    Transform currentFollow;
    private void Start() {
        currentFollow = followTarget;
    }

    private void Update() {
        if (isFollowing) {
            //Follwing target transform
            transform.position = Vector3.Lerp(transform.position, currentFollow.position, followingSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, currentFollow.rotation, followingSpeed);
        }
    }

    //Most likely a TEMPERARY FUNCTION used to put the sword in the right place for attacks before we have animations
    override public void StabPosition(float duration) {
        currentFollow = stabTransform;
        isAttacking = true;
        Invoke("EndAttackPosition", duration);
    }
    override public void SlashPosition(float duration) {
        currentFollow = slashTransform;
        isAttacking = true;
        Invoke("EndAttackPosition", duration);
    }
    override public void DownwardAttackPosition() {
        currentFollow = downwardStabTransform;
        isAttacking = true;
    }
    override public void DashAttackPosition() {
        currentFollow = dashAttackTransform;
        isAttacking = true;
    }
    override public void EndAttackPosition() {
        currentFollow = followTarget;
        isAttacking = false;
        CancelInvoke();
        OnEndAction.Invoke();
    }
}
