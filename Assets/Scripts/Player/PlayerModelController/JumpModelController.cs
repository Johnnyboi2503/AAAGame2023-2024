using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpModelController : MonoBehaviour
{
    [SerializeField] Animator jumpAnimator;

    private void OnEnable() {
        jumpAnimator.SetTrigger("Jump");
    }
}
