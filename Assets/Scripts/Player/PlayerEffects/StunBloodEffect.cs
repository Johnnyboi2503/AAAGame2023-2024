using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunBloodEffect : MonoBehaviour {
    [SerializeField] Collider playerCollider;
    [SerializeField] PlayerStun stun;
    EffectsController effects;

    private void Start() {
        effects = FindObjectOfType<EffectsController>();
        stun.OnStun.AddListener(SpawnBlood);
    }
    public void SpawnBlood() {
        Vector3 initalPos = Vector3.Scale(Random.insideUnitSphere, playerCollider.bounds.extents);
        Vector3 direction = transform.position - initalPos;

        effects.CreateBloodEffect(transform.position+initalPos, direction, false);
    }
}
