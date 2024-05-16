using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyStunState : MeleeEnemyBaseState
{

    public Animator animator;
    public float stunDuration;
    public float stunTimer;
    public ParticleSystem particle;

    public MeleeEnemyStunState(float _stunDuration) {
        stunDuration = _stunDuration;
    }

    public override void EnterState(MeleeEnemyStateManager enemy) {
        stunTimer = 0;
        //Debug.Log("Enter Stun State");
        enemy.render.material.color = Color.red;
        Debug.Log("STUNNED");
        enemy.particle.Play();
        enemy.animator.speed = 0;
        
    }

    public override void UpdateState(MeleeEnemyStateManager enemy) {
        stunTimer += Time.deltaTime;
        if (stunTimer >= stunDuration) {
            enemy.Idle();
            enemy.animator.speed = 1;

        }
        //Debug.Log("Enter Stun Update");
        // no longer moving
        enemy.StopPosition();
        //Debug.Log("stunned");
        // after amount of time no longer stunned
    

    }
}
