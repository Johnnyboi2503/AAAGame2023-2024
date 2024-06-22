using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwordMovement : MonoBehaviour {
    [Header("Player Positions")]
    [SerializeField] Transform followTarget;
    [SerializeField] Transform stabTransform;
    [SerializeField] Transform slashTransform;
    [SerializeField] Transform downwardStabTransform;
    [SerializeField] Transform dashAttackTransform;
    [SerializeField] Transform freeTransform;

    [Header("Other Variables")]
    [Range(0f, 1f)]
    [SerializeField] float followingSpeed;
    [SerializeField] int playerLayerNumber;
    [SerializeField] int notAttackableLayer;
    [SerializeField] float slideOffsetFromSurface;
    [SerializeField] float slideOffsetFromParent;
    [SerializeField] float dashThroughOffset;

    [Header("Events")]
    public UnityEvent<Collider> OnContact = new UnityEvent<Collider>();
    public UnityEvent OnEndAction = new UnityEvent();

    [Header("References")]
    [SerializeField] SlashContact slashContact;
    [SerializeField] StabContact stabContact;

    // Other variables
    private bool isFollowing = true;
    public bool isAttacking = false;
    Transform currentFollow;

    // Blood effect variables
    [Header("Blood Effect Variables")]
    [SerializeField] float bloodRate;
    EffectsController effect;
    GameObject currentBloodEffect = null;
    
    private void Start() {
        currentFollow = followTarget;
        effect = FindObjectOfType<EffectsController>();
    }

    private void Update() {
        if (isFollowing) {
            //Follwing target transform
            transform.position = Vector3.Lerp(transform.position, currentFollow.position, followingSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, currentFollow.rotation, followingSpeed);
        }

    }

    //Most likely a TEMPERARY FUNCTION used to put the sword in the right place for attacks before we have animations
    public void StabPosition(float duration) {
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
        // Blood effect
        if(currentBloodEffect != null) {
            Destroy(currentBloodEffect);
            currentBloodEffect = null;
        }

        // Setting other values
        currentFollow = followTarget;
        isAttacking = false;
        CancelInvoke();
        OnEndAction.Invoke();
    }

    public void UpdateSlidePosition(Vector3 parentPos, Collider slidingCollider, Vector3 pathDir) {

        // Calculating contact point + surface normal
        Vector3 contactCheckOffset = parentPos + (pathDir.normalized * slideOffsetFromParent);
        Vector3 contactPoint = slidingCollider.ClosestPoint(contactCheckOffset);
        Vector3 surfaceNormal = (contactCheckOffset - contactPoint).normalized;

        // Applying values to rotation transform
        freeTransform.position = contactPoint + (slideOffsetFromSurface * surfaceNormal);
        freeTransform.up = -surfaceNormal;

        // Setting follow
        currentFollow = freeTransform;

        BloodEffectUpdate();
    }
    public void UpdateDashThrough(Vector3 parentPos, Vector3 direction) {
        freeTransform.position = parentPos + (direction.normalized * dashThroughOffset);
        freeTransform.up = direction.normalized;

        currentFollow = freeTransform;

        BloodEffectUpdate(true);
    }
    public void UpdateFlick(Vector3 parentPos, Collider slidingCollider) {
        freeTransform.position = slidingCollider.ClosestPoint(parentPos);
        freeTransform.up = (freeTransform.position - parentPos).normalized;

        currentFollow = freeTransform;

        BloodEffectUpdate();
    }

    private void BloodEffectUpdate(bool invertDir = false) {
        Vector3 dir = -freeTransform.up;
        if(invertDir) { dir = -dir; }
        
        // Creating and setting blood effect
        if (currentBloodEffect == null) {
            currentBloodEffect = effect.CreateBloodEffect(freeTransform.position, dir, true, bloodRate);
        }
        else {
            currentBloodEffect.transform.position = freeTransform.position;
            effect.SetDirection(currentBloodEffect, dir);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (slashContact.CanSlash(other.gameObject) || stabContact.CanStab(other.gameObject)) {
            if (other.gameObject.layer != gameObject.layer && other.gameObject.layer != notAttackableLayer && isAttacking) {
                OnContact.Invoke(other);
                EndAttackPosition();
            }
        }
    }
}
