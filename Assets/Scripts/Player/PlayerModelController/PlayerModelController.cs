using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelController : MonoBehaviour
{
    [SerializeField] Animator playerModelController;
    [SerializeField] Rigidbody rb;

    // Actions
    [Header("Dash References")]
    [SerializeField] DashAction dash;
    [SerializeField] StabDashAction stabDash;
    [SerializeField] SlashDashAction slashDash;

    [Header("Curve Flow References")]
    [SerializeField] SlideAction slide;
    [SerializeField] DashThroughAction dashThrough;
    [SerializeField] DownwardStabAction downwardStab;
    [SerializeField] FlickAction flick;

    // Other references
    [Header("Other References")]
    [SerializeField] PlayerStun stun;

    private void Start() {
        // Dash animations
        dash.OnStartAction.AddListener(SetDashTrue);
        dash.OnEndAction.AddListener(SetDashFalse);
        stabDash.OnStartAction.AddListener(SetDashTrue);
        stabDash.OnEndAction.AddListener(SetDashFalse);
        slashDash.OnStartAction.AddListener(SetDashTrue);
        slashDash.OnEndAction.AddListener(SetDashFalse);

        // Curve animations
        slide.OnStartAction.AddListener(SetCurveTrue);
        slide.OnEndAction.AddListener(SetCurveFalse);
        dashThrough.OnStartAction.AddListener(SetCurveTrue);
        dashThrough.OnEndAction.AddListener(SetCurveFalse);
        downwardStab.OnStartAction.AddListener(SetCurveTrue);
        downwardStab.OnEndAction.AddListener(SetCurveFalse);
        flick.OnStartAction.AddListener(SetCurveTrue);
        flick.OnEndAction.AddListener(SetCurveFalse);


        // take damage
        stun.OnStun.AddListener(SetDamageTrue);
        stun.OnEndStun.AddListener(SetDamageFalse);
    }

    private void Update() {
        Vector3 horzVelocity = rb.velocity;
        horzVelocity.y = 0;

        playerModelController.SetFloat("HorzSpeed", horzVelocity.magnitude);
        playerModelController.SetFloat("VertSpeed", rb.velocity.y);
    }

    public void SetDashTrue() {
        SetDash(true);
    }
    public void SetDashFalse() {
        SetDash(false);
    }

    public void SetCurveTrue() {
        SetCurveFlow(true);
    }
    public void SetCurveFalse() {
        SetCurveFlow(false);
    }

    public void SetDamageTrue() {
        SetTakeDamage(true);
    }
    public void SetDamageFalse() {
        SetTakeDamage(false);
    }
    public void SetDash(bool set) {
        playerModelController.SetBool("Dash", set);
    }
    public void SetCurveFlow(bool set) {
        playerModelController.SetBool("CurveFlow", set);
    }
    public void SetTakeDamage(bool set) {
        playerModelController.SetBool("TakeDamage", set);
    }
}
