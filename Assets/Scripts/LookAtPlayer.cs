using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] float stickMag;
    Transform playerTransform;

    private void Start() {
        playerTransform = FindObjectOfType<PlayerInput>().transform;
    }

    private void Update() {
        if(playerTransform != null) {
            Vector3 pointToPlayer = playerTransform.position - transform.position;
            transform.forward = Vector3.Lerp(transform.forward, pointToPlayer.normalized, stickMag);
        }
    }
}
