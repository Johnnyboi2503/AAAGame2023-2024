using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] [Range(0f,1f)]float followMag;
    bool following = true;

    // Update is called once per frame
    void Update()
    {
        if(following && target != null) {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, target.position, followMag);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, target.rotation, followMag);
        }
    }
    public void SetTarget(Transform set) {
        target = set;
    }

    public void SetFollow(bool set) {
        following = set;
    }
}
