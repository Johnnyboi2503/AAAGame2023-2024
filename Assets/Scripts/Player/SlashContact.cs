using PathCreation;
using UnityEngine;
using UnityEngine.Events;

public class SlashContact : MonoBehaviour {

    [SerializeField] private float wallSlashAudioVolume = 0.75f;

    // Action to perform
    SlideAction slideAction;

    // For resets
    JumpAction jumpAction;
    DashAction dashAction;

    SlidableEnemy slidableEnemy;

    // Blood Effect
    EffectsController effect;

    // Other variables
    float bloodGainAmount;
    UnityEvent<Collider> contactEvent;

    private void Start() {
        slideAction = GetComponentInParent<SlideAction>();
        jumpAction = GetComponentInParent<JumpAction>();
        dashAction = GetComponentInParent<DashAction>();

        // End of events
        slideAction.OnEndAction.AddListener(EndOfSlide);

        effect = FindObjectOfType<EffectsController>();
    }

    private void StabContactEffect(Collider other) {

        bool found = false;
        bool canGainBlood = false;

        Vector3 slashDirection = transform.forward;

        if (other.TryGetComponent(out Slashable slashable)) {
            slashable.TriggerEffect();
            found = true;
        }
        
        if(other.TryGetComponent(out SlidableEnemy enemy)) {
            slidableEnemy = enemy;
            StartSlideAction(enemy.pathCreator, other, enemy.GetBonus());
            found = true;

            // Blood effect
            Vector3 point = other.ClosestPoint(transform.position);
            Vector3 dir = transform.position - point;
            effect.CreateBloodEffect(point, dir, false);
        }
        else if (other.TryGetComponent(out PathCreator pathCreator)) {
            AudioManager.GetInstance().PlayAudioAtLocation("WallSlash_SFX", transform.position, wallSlashAudioVolume);
            if (other.TryGetComponent(out WallDirection wallDir)) {
                Vector3 wallForward = wallDir.GetForwardVector();

                //Dot gives a value comparing the two directions, 1 = same direction, -1 = opposite direction
                float directionCheck = Vector3.Dot(slashDirection.normalized, wallForward.normalized);

                if (directionCheck < 0) {
                    StartSlideAction(pathCreator, other);
                    found = true;
                }
            }
            else {
                StartSlideAction(pathCreator, other);
                found = true;
            }

            // Blood effect
            Vector3 point = other.ClosestPoint(transform.position);
            Vector3 dir = transform.position - point;
            effect.CreateBloodEffect(point, dir, false);
        }
        if (other.TryGetComponent(out SlashableEnviornment slashableEnviornment)) {
            canGainBlood = slashableEnviornment.canGiveBlood;
        }
        if(found) {
            EndContactEvent();
        }
        if(canGainBlood) {
            GetComponentInParent<BloodThirst>().GainBlood(bloodGainAmount, true);
        }
    }

    public bool CanSlash(GameObject check) {
        return check.TryGetComponent(out Slashable slashable) ||
            check.TryGetComponent(out SlidableEnemy slidableEnemy) ||
            check.TryGetComponent(out PathCreator pathCreator) ||
            check.TryGetComponent(out SlashableEnviornment slashableEnviornment);
    }

    private void StartSlideAction(PathCreator pc, Collider other, float enemyBonus = 0) {
        GetComponent<PlayerActionManager>().ChangeAction(slideAction);
        slideAction.OnEndAction.AddListener(EndEnemy);
        slideAction.StartSlide(pc, other, enemyBonus);
    }

    private void EndEnemy() {
        if (slidableEnemy != null) {
            slidableEnemy.Die();
        }
        slideAction.OnEndAction.RemoveListener(EndEnemy);
    }

    public void ActivateContactEvent(UnityEvent<Collider> _contactEvent, float bloodGained) {
        bloodGainAmount = bloodGained; 
        contactEvent = _contactEvent;
        contactEvent.AddListener(StabContactEffect);
    }

    public void EndContactEvent() {
        contactEvent.RemoveListener(StabContactEffect);
    }

    private void EndOfSlide() {
        jumpAction.GiveAirJump();
        dashAction.ResetDash();
    }
}
