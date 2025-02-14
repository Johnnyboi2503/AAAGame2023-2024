using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabableEnviornment : StabableDashThrough {
    [Header("References")]
    public Transform dashEndTransform;

    private void OnDrawGizmos() {
        CalculateDash(this.gameObject, false);

        DrawArrow(transform.position, dashEndTransform.position);
    }

    public override void CalculateDash(GameObject source, bool playAudio) {
        dashDir = dashEndTransform.position - transform.position;
        dashLength = dashDir.magnitude;
        if (playAudio) {
            AudioManager.GetInstance().PlayAudioFollowObject("StabableCubeLaunchSFX", source, 1.0f);
        }
    }


    private void DrawArrow(Vector3 start, Vector3 end) {
        //makes it so the gizmos transform with the local transform of the object
        //Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;

        //direction is the direction and distance the player will dash through
        Vector3 displayDirection = end - start;
        //ray starts behind transform.position so you can see it kinda go through the object
        Gizmos.DrawRay(start, displayDirection);
        //handleLength determines how long the arrow handles are
        float handleLength = .5f;
        //got this math off the internet so i dont understand it too well but hey it works! x_x
        Vector3 rightHandle = Quaternion.LookRotation(displayDirection) * Quaternion.Euler(0, 200, 0) * new Vector3(0, 0, 1);
        Vector3 leftHandle = Quaternion.LookRotation(displayDirection) * Quaternion.Euler(0, 160, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(start + displayDirection, rightHandle * handleLength);
        Gizmos.DrawRay(start + displayDirection, leftHandle * handleLength);
    }
}
