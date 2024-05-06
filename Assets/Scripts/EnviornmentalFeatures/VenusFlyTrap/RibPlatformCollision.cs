using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibPlatformCollision : MonoBehaviour
{
    [SerializeField] RibPlatform platform;

    private void OnTriggerStay(Collider other) {
        if (other.TryGetComponent(out PlayerInput playerInput)) {
            platform.FoundPlayer(playerInput); // Triggering the collision on player
        }
    }
}
