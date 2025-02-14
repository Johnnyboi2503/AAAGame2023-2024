using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StabContact : MonoBehaviour
{
    [SerializeField] private float wallStabAudioVolume = 0.75f;

    // Contact Action
    DashThroughAction dashThroughAction;
    FlickAction flickAction;

    // For resets
    JumpAction jumpAction;
    DashAction dashAction;

    DashThroughEnemy stabableThrough;

    float bloodGainAmount;
    UnityEvent<Collider> contactEvent;

    // Blood Effect
    EffectsController effect;

    private void Start() {
        dashThroughAction = GetComponentInParent<DashThroughAction>();
        flickAction = GetComponentInParent<FlickAction>();
        jumpAction = GetComponentInParent<JumpAction>();
        dashAction = GetComponentInParent<DashAction>();

        // End of events
        dashThroughAction.OnEndAction.AddListener(EndOfDash);

        effect = FindObjectOfType<EffectsController>();
    }

    private void StabContactEffect(Collider other) {
        bool found = false;
        bool canGiveBlood = false;

        PlayerActionManager actionManager = GetComponent<PlayerActionManager>();
        if (other.gameObject.TryGetComponent(out Stabable stabable)) {
            found = true;
            stabable.TriggerEffect();
        }
        if(other.gameObject.TryGetComponent(out DashThroughEnemy dashThroughEnemy)) {
            stabableThrough = dashThroughEnemy;

            canGiveBlood = dashThroughEnemy.canGiveBlood;

            // Setting up and starting dash
            dashThroughAction.OnEndAction.AddListener(EndEnemy);
            actionManager.ChangeAction(dashThroughAction);
            dashThroughAction.DashThrough(dashThroughEnemy, dashThroughEnemy.GetBonus());

            // Blood effect
            Vector3 point = other.ClosestPoint(transform.position);
            Vector3 dir = point - transform.position;
            effect.CreateBloodEffect(point, dir, false);
        }
        else if (other.gameObject.TryGetComponent(out StabableDashThrough dashThrough)) {
            canGiveBlood = dashThrough.canGiveBlood;

            // Setting up and starting dash
            actionManager.ChangeAction(dashThroughAction);
            dashThroughAction.DashThrough(dashThrough);

            found = true;

            AudioManager.GetInstance().PlayAudioFollowObject("WallStab_SFX", gameObject, wallStabAudioVolume);

            // Blood effect
            Vector3 point = other.ClosestPoint(transform.position);
            Vector3 dir = transform.position - point;
            effect.CreateBloodEffect(point, dir, false);
        }
        if(other.gameObject.TryGetComponent(out FlickEnemyStabable flickEnemy)) {
            actionManager.ChangeAction(flickAction);
            flickAction.Stick(flickEnemy, null);

            // Blood effect
            Vector3 point = other.ClosestPoint(transform.position);
            Vector3 dir = transform.position - point;
            effect.CreateBloodEffect(point, dir, false);
        }
        if(other.gameObject.TryGetComponent(out FlickEnviornmentStabable flickEnviornment)) {
            actionManager.ChangeAction(flickAction);
            flickAction.Stick(null, flickEnviornment);

            // Blood effect
            Vector3 point = other.ClosestPoint(transform.position);
            Vector3 dir = transform.position - point;
            effect.CreateBloodEffect(point, dir, false);
        }
        if(found) {
            EndContactEvent();
        }
        if(canGiveBlood) {
            GetComponentInParent<BloodThirst>().GainBlood(bloodGainAmount, true);
        }
    }

    public bool CanStab(GameObject check) {
        return check.TryGetComponent(out Stabable stabable) ||
            check.TryGetComponent(out StabableDashThrough dashThrough) ||
            check.TryGetComponent(out FlickEnemyStabable flickEnemy) ||
            check.TryGetComponent(out FlickEnviornmentStabable flickEnviornment);
    }

    public void ActivateContactEvent(UnityEvent<Collider> _contactEvent, float bloodGained) {
        bloodGainAmount = bloodGained;
        contactEvent = _contactEvent;
        contactEvent.AddListener(StabContactEffect);
    }

    private void EndEnemy() {
        if (stabableThrough != null) {
            stabableThrough.Die();
        }
        dashThroughAction.OnEndAction.RemoveListener(EndEnemy);
    }

    public void EndContactEvent() {
        contactEvent.RemoveListener(StabContactEffect);
    }

    private void EndOfDash() {
        jumpAction.GiveAirJump();
        dashAction.ResetDash();
    }
}
