using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStun : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] PlayerActionManager actionManager;
    [SerializeField] Renderer render;
    [SerializeField] Color stunColor;

    public UnityEvent OnStun = new UnityEvent();
    public UnityEvent OnEndStun = new UnityEvent();

    float stunTimer;
    bool isStunned = false;
    Color holderColor;
    private void Start() {
        holderColor = render.material.color;
    }

    private void Update() {
        if (isStunned) {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0) {
                // Setting value
                isStunned = false;
                render.material.color = holderColor;

                // Enabling player
                input.EnableAbilityInput();
                OnEndStun.Invoke();
            }
        }
    }
    public void Stun(float duration) {
        // Disabling player
        actionManager.EndCurrentAction();
        input.DisableAbilityInput();

        // Setting timer + value
        stunTimer = duration;
        isStunned = true;

        render.material.color = stunColor;

        OnStun.Invoke();
    }

    public bool IsStunned() {
        return stunTimer > 0;
    }
}
