using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashMovement {
    // References needed
    Rigidbody rb;
    Transform transform;

    // temperary variables
    public UnityEvent OnDashEnd = new UnityEvent();
    float dashDurationTimer;
    public Vector3 dashVelocity;
    Vector3 endDashVelocity;
    float dragValHolder;
    public bool isDashing = false;

    public DashMovement(Transform _transform, Rigidbody _rb) {
        transform = _transform;
        rb = _rb;
    }

    public void Dash(float speed, float duration, Vector3 direction, float endDashSpeed) {
        // Default value for no direction given
        if (direction.magnitude == 0) {
            direction = transform.forward;
        }

        // Setting variables
        dashDurationTimer = duration; // starting dash duration timer
        dashVelocity = speed * direction.normalized; // setting velocity for dashing
        endDashVelocity = endDashSpeed * direction.normalized;

        // Set up for physics variables
        isDashing = true;
        dragValHolder = rb.drag;
        rb.useGravity = false;
        rb.velocity = dashVelocity;
    }

    // This function needs to be constantly run during the update function
    public void UpdateDashing() {
        if (isDashing) {
            // Allowing dashing as long as the dash direction is no in the same direction as the current velocity
            if (dashDurationTimer > 0) {
                rb.drag = 0;
                rb.velocity = dashVelocity;
            }
            else {
                EndOfDash();
            }
            dashDurationTimer -= Time.fixedDeltaTime;
        }
    }
    public void InteruptDash() {
        rb.drag = dragValHolder;
        rb.useGravity = true;
        rb.velocity = endDashVelocity;

        isDashing = false;
    }

    private void EndOfDash() {
        //Rstoring movement variables
        InteruptDash();
        OnDashEnd.Invoke();
    }
}
