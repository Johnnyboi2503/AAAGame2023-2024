using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class BloodLifeTime : MonoBehaviour
{
    [SerializeField] VisualEffect effect;
    [SerializeField] float destoryDelay;
    [SerializeField] float MAX_LIFETIME;
    float timer = 0;

    // Update is called once per frame
    void Update() {
        if (!effect.GetBool("Loop")) {
            Invoke("DestroySelf", destoryDelay);
        }
        else {
            timer += Time.deltaTime;
            if(timer >= MAX_LIFETIME) {
                DestroySelf();
            }
        }
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}
