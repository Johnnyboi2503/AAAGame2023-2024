using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestModelPoint : MonoBehaviour
{
    [SerializeField] List<Transform> armature = new List<Transform>();
    [SerializeField] float updateRate;
    [SerializeField] Transform pointTo;


    float timer;

    private void Update() {
        timer -= Time.deltaTime;
        if(timer <= 0) {
            foreach(Transform bone in armature) {
                Vector3 dir = pointTo.position - bone.position;
                bone.rotation = Quaternion.LookRotation(dir, Vector3.up);
            }
            timer = updateRate;
        }
    }
}
