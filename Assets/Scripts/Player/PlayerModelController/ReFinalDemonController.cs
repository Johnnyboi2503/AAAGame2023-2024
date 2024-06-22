using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReFinalDemonController : MonoBehaviour
{
    [SerializeField] Animator reModel;
    [SerializeField] GameObject armatureParent; 
    [SerializeField] bool idle = false;
    [SerializeField] bool running = false;
    List<Vector3> positions = new List<Vector3>();
    List<Quaternion> rotations = new List<Quaternion>();
    List<Transform> bones = new List<Transform>();

    private void Start() {
        bones = new List<Transform>(armatureParent.GetComponentsInChildren<Transform>());
        foreach(Transform bone in bones) {
            positions.Add(bone.transform.localPosition);
            rotations.Add(bone.transform.localRotation);
        }
    }

    private void Update() {
        UpdateBool();
    }

    public void UpdateBool() {
        reModel.SetBool("Idle", idle);
        reModel.SetBool("Running", running);
    }

    public void ResetArmature() {
        for(int i = 0; i < bones.Count; i++) {
            bones[i].localPosition = positions[i];
            bones[i].localRotation = rotations[i];
        }
    }
}
