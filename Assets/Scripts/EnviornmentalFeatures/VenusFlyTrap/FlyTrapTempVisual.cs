using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTrapTempVisual : MonoBehaviour
{
    [SerializeField] GameObject jaw1;
    [SerializeField] GameObject jaw2;
    [SerializeField] Transform close1;
    [SerializeField] Transform close2;
    [SerializeField] Transform open1;
    [SerializeField] Transform open2;
    [SerializeField] float lerpVal;
    [SerializeField] float shakeMag;

    Transform target1;
    Transform target2;

    Quaternion saveRot1;
    Quaternion saveRot2;
    bool shaking = false;
    private void Start() {
        target1 = open1;
        target2 = open2;
    }

    private void Update() {
        if (shaking) {
            jaw1.transform.rotation = Quaternion.Euler(saveRot1.eulerAngles + Random.insideUnitSphere);
            jaw2.transform.rotation = Quaternion.Euler(saveRot2.eulerAngles + Random.insideUnitSphere);
        }
        else {
            JawUpdate(jaw1, target1);
            JawUpdate(jaw2, target2);
        }
    }
    private void JawUpdate(GameObject jaw, Transform target) {
        jaw.transform.position = Vector3.Lerp(jaw.transform.position, target.position, lerpVal);
        jaw.transform.rotation = Quaternion.Lerp(jaw.transform.rotation, target.rotation, lerpVal);
    }
    public void Close() {
        target1 = close1;
        target2 = close2;
    }
    public void Open() {
        target1 = open1;
        target2 = open2;
    }
    public void Shake() {
        saveRot1 = jaw1.transform.rotation;
        saveRot2 = jaw2.transform.rotation;
        shaking = true;
    }
    public void StopShake() {
        shaking = false;
    }
}
