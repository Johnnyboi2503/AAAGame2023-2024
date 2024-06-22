using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdollConnector : MonoBehaviour
{
    [SerializeField] PlayerRagDollController controller;
    [SerializeField] Transform swordHilt;
    [SerializeField] SlideAction slideAction;

    private void Start() {
        slideAction.OnStartAction.AddListener(StartAction);
        slideAction.OnEndAction.AddListener(EndAction);
    }

    public void StartAction() {
        controller.EnableRagDoll();
        controller.AssignHinge(swordHilt);
        //controller.SetLeftArmTarget(swordHilt.position);
        //controller.EnableConstraint();
    }
    public void EndAction() {
        //controller.DisableConstraint();
        controller.DisableRagDoll();
        controller.DestroyHinge();
    }
}
