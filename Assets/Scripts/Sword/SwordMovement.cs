using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SwordMovement : MonoBehaviour
{
    [Header("Other Variables")]
    [SerializeField] List<int> attackableLayers;

    [Header("Events")]
    public UnityEvent<Collider> OnContact = new UnityEvent<Collider>();
    public UnityEvent OnEndAction = new UnityEvent();

    // Other variables
    protected bool isAttacking = false;

    //Most likely a TEMPERARY FUNCTION used to put the sword in the right place for attacks before we have animations
    abstract public void StabPosition(float duration);
    abstract public void SlashPosition(float duration);
    abstract public void DownwardAttackPosition();
    abstract public void DashAttackPosition();
    abstract public void EndAttackPosition();

    public bool IsAttacking() {
        return isAttacking;
    }

    private void OnTriggerStay(Collider other) {
        // Checking all layers
        if (other.gameObject.layer != gameObject.layer) {
            foreach (int layer in attackableLayers) {
                if (other.gameObject.layer == layer) {
                    OnContact.Invoke(other);
                    EndAttackPosition();
                }
            }
        }
    }
}
