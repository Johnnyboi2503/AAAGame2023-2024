using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Killable : MonoBehaviour
{
    [Header("Health Variables")]
    public float maxHP; 
    public float currentHP;
    public bool isKillable = true;
    public bool isDead = false;

    [Header("Events")]
    public UnityEvent OnHealthChange = new UnityEvent(); // Event that occurs when current health value changes
    public UnityEvent OnTakeDamage = new UnityEvent(); // Event that occurs when object takes damage
    public UnityEvent OnDie = new UnityEvent(); // Event that occurs when current hp < 0 which means dead

    public void TakeDamage(float amount)
    {
        currentHP = Mathf.Max(currentHP-amount, 0); // Limitting to 0
        OnTakeDamage.Invoke();
        OnHealthChange.Invoke();

        if(currentHP <= 0 && isKillable)
        {
            isDead = true;
            OnDie.Invoke();
        }
    }

    public void GainHealth(float amount)
    {
        //Increasing hp while limiting it to the the max
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        OnHealthChange.Invoke();
    }

    public void Revive() {
        isDead = false;
        GainHealth(maxHP);
    }
}
