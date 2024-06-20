using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectsController : MonoBehaviour {
    [SerializeField] GameObject bloodEffectPrefab;
    // Testing
    [SerializeField] Vector3 velocity;
    [SerializeField] bool loop;
    [SerializeField] bool allowTestInput;

    private void Update() {
        if (allowTestInput) {
            InputTestUpdate();
        }
    }

    private void InputTestUpdate() {
        // Testing
        if (Input.GetKeyDown(KeyCode.V)) {
            CreateBloodEffect(transform.position, velocity, loop);
        }
    }

    public GameObject CreateBloodEffect(Vector3 position, Vector3 direction, bool loop) {
        GameObject currentEffect = Instantiate(bloodEffectPrefab, position, Quaternion.identity);
        VisualEffect settings = currentEffect.GetComponentInChildren<VisualEffect>();
        settings.Stop();

        currentEffect.transform.forward = direction;
        settings.SetBool("Loop", loop);
        
        settings.Play();

        return currentEffect;
    }
}
