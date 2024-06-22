using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectsController : MonoBehaviour {
    [SerializeField] GameObject bloodEffectPrefab;
    // Testing
    [SerializeField] bool allowTestInput;
    [SerializeField] Vector3 defaultDirection = Vector3.up;
    [SerializeField] const bool defaultLoop = false;
    [SerializeField] const float defaultRate = 32;
    [SerializeField] const float defaultSpeed = 5f;
    [SerializeField] const float defaultSpread = 0.5f;
    [SerializeField] const float circleBurstSpread = 2f;

    private void Update() {
        if (allowTestInput) {
            InputTestUpdate();
        }
    }

    private void InputTestUpdate() {
        // Testing
        if (Input.GetKeyDown(KeyCode.V)) {
            CreateBloodEffect(transform.position, defaultDirection, defaultLoop);
        }
    }

    public GameObject CreateBloodEffect(Vector3 position, Vector3 direction, bool loop = defaultLoop, float rate = defaultRate, float speed = defaultSpeed, float spread = defaultSpread) {
        GameObject currentEffect = Instantiate(bloodEffectPrefab, position, Quaternion.identity);
        VisualEffect settings = currentEffect.GetComponentInChildren<VisualEffect>();
        settings.Stop();

        currentEffect.transform.forward = direction.normalized;
        settings.SetBool("Loop", loop);
        settings.SetFloat("Rate", rate);
        settings.SetFloat("BloodSpeed", speed);
        settings.SetFloat("BloodSpread", spread);

        settings.Play();

        return currentEffect;
    }

    public void SetDirection(GameObject effectObject, Vector3 dir) {
        effectObject.transform.forward = dir.normalized;
    }

    public void CircleBurst(Vector3 position) {
        CreateBloodEffect(position, Vector3.up, false, defaultRate, defaultSpeed, circleBurstSpread);
    }
}
