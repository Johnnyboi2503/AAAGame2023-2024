using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwordMovement : MonoBehaviour
{
    [Header("Player Positions")]
    [SerializeField] Transform followTarget;
    [SerializeField] Transform stabTransform;
    [SerializeField] Transform slashTransform;
    [SerializeField] Transform downwardStabTransform;
    [SerializeField] Transform dashAttackTransform;
    [SerializeField] Transform slideTransform;

    [Header("Other Variables")]
    [Range(0f,1f)]
    [SerializeField] float followingSpeed;
    [SerializeField] int playerLayerNumber;
    [SerializeField] int notAttackableLayer;
    [SerializeField] float slideOffsetFromSurface;
    [SerializeField] float slideOffsetFromParent;
    [SerializeField] float dashThroughOffset;

    [Header("Events")]
    public UnityEvent<Collider> OnContact = new UnityEvent<Collider>();
    public UnityEvent OnEndAction = new UnityEvent();

    // Other variables
    private bool isFollowing = true;
    public bool isAttacking = false;
    Transform currentFollow;

    private void Start() {
        currentFollow = followTarget;
    }

    private void Update()
    {
        if (isFollowing)
        {
            //Follwing target transform
            transform.position = Vector3.Lerp(transform.position, currentFollow.position, followingSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, currentFollow.rotation, followingSpeed);
        }

    }

    //Most likely a TEMPERARY FUNCTION used to put the sword in the right place for attacks before we have animations
    public void StabPosition(float duration)
    {
        currentFollow = stabTransform;
        isAttacking = true;
        Invoke("EndAttackPosition", duration);
    }
    public void SlashPosition(float duration) {
        currentFollow = slashTransform;
        isAttacking = true;
        Invoke("EndAttackPosition", duration);
    }
    public void DownwardAttackPosition() {
        currentFollow = downwardStabTransform;
        isAttacking = true;
    }
    public void DashAttackPosition() {
        currentFollow = dashAttackTransform;
        isAttacking = true;
    }
    public void EndAttackPosition() {
        currentFollow = followTarget;
        isAttacking = false;
        CancelInvoke();
        OnEndAction.Invoke();
    }
    public void UpdateSlidePosition(Vector3 parentPos, Collider slidingCollider, Vector3 pathDir) {
        // Calculating contact point + surface normal
        Vector3 contactCheckOffset = parentPos + (pathDir.normalized*slideOffsetFromParent);
        Vector3 contactPoint = slidingCollider.ClosestPoint(contactCheckOffset);
        Vector3 surfaceNormal = (contactCheckOffset - contactPoint).normalized;

        // Applying values to rotation transform
        slideTransform.position = contactPoint + (slideOffsetFromSurface*surfaceNormal);
        slideTransform.up = -surfaceNormal;
        
        // Setting follow
        currentFollow = slideTransform;
    }

    public void UpdateDashThrough(Vector3 parentPos, Vector3 direction) {
        slideTransform.position = parentPos + (direction.normalized * dashThroughOffset);
        slideTransform.up = direction.normalized;

        currentFollow = slideTransform;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != gameObject.layer && other.gameObject.layer != notAttackableLayer && isAttacking) {
            OnContact.Invoke(other);
            EndAttackPosition();
        }
    }
}
