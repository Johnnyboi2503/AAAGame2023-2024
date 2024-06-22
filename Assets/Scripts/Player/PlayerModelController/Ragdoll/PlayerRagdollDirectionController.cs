using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdollDirectionController : MonoBehaviour {
    [SerializeField] GameObject model;
    [SerializeField] List<GameObject> selectObjects = new List<GameObject>();
    [SerializeField] LayerMask ignoreLayers;
    [SerializeField] [Range(0f, 1f)] float transitionMag;
    [SerializeField] float transitionDuration;
    [SerializeField] float forceApplied;
    [SerializeField] bool useSelectObjects;
    [SerializeField] List<Rigidbody> targetForceAppliedRB = new List<Rigidbody>();

    List<Vector3> targetPosition = new List<Vector3>();
    List<Quaternion> targetRotation = new List<Quaternion>();
    List<Rigidbody> ragdollRididbodies = new List<Rigidbody>();
    List<Collider> ragdollColliders = new List<Collider>();

    float transitionTimer;
    Vector3 pointDir = Vector3.down;

    enum PlayerRagDollState {
        TRANSITION,
        RAGDOLL,
        NORAGDOLL
    }

    PlayerRagDollState ragdollState = PlayerRagDollState.NORAGDOLL;


    private void Start() {
        // Freezing all rbs
        foreach (Rigidbody rb in model.GetComponentsInChildren<Rigidbody>()) {
            rb.isKinematic = true;
        }

        // Getting compoenents
        if (useSelectObjects) {
            foreach (GameObject selectObject in selectObjects) {
                ragdollRididbodies.Add(selectObject.GetComponent<Rigidbody>());
                ragdollColliders.Add(selectObject.GetComponent<Collider>());
            }
        }
        else {
            ragdollRididbodies = new List<Rigidbody>(model.GetComponentsInChildren<Rigidbody>());
            ragdollColliders = new List<Collider>(model.GetComponentsInChildren<Collider>());
        }

        foreach (Rigidbody rb in ragdollRididbodies) {
            // Saving previous position
            targetPosition.Add(rb.transform.localPosition);
            targetRotation.Add(rb.transform.localRotation);

            // Setting ignore layers
            rb.excludeLayers = ignoreLayers;
        }

        EnableRagDoll();
    }

    private void FixedUpdate() {
        if (ragdollState == PlayerRagDollState.RAGDOLL) {
            foreach (Rigidbody rb in targetForceAppliedRB) {
                rb.velocity += pointDir.normalized * forceApplied * Time.fixedDeltaTime;
            }
        }
    }
    private void Update() {
        if (Input.GetKey(KeyCode.M)) {
            DisableRagDoll();
        }
        if (Input.GetKey(KeyCode.N)) {
            EnableRagDoll();
        }

        switch (ragdollState) {
            // Transitioning from ragdoll to no ragdoll
            case PlayerRagDollState.TRANSITION:

                // Lerping position and rotation
                for (int i = 0; i < ragdollRididbodies.Count; i++) {
                    if (targetPosition.Count > i) {
                        ragdollRididbodies[i].transform.localPosition = Vector3.Lerp(ragdollRididbodies[i].transform.localPosition, targetPosition[i], transitionMag);
                        ragdollRididbodies[i].transform.localRotation = Quaternion.Lerp(ragdollRididbodies[i].transform.localRotation, targetRotation[i], transitionMag);
                    }
                }

                transitionTimer -= Time.deltaTime;
                if (transitionTimer <= 0) {
                    ragdollState = PlayerRagDollState.NORAGDOLL;
                }
                break;
        }
    }

    public void SetPointDir(Vector3 direction) {
        pointDir = direction;
    }

    public void EnableRagDoll() {

        foreach (Rigidbody rb in ragdollRididbodies) {
            rb.isKinematic = false;
        }
        foreach (Collider col in ragdollColliders) {
            col.isTrigger = false;
        }

        ragdollState = PlayerRagDollState.RAGDOLL;
    }

    public void DisableRagDoll() {
        foreach (Rigidbody rb in ragdollRididbodies) {
            rb.isKinematic = true;
        }
        foreach (Collider col in ragdollColliders) {
            col.isTrigger = true;
        }

        ragdollState = PlayerRagDollState.TRANSITION;
        transitionTimer = transitionDuration;
    }
}
