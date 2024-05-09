using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunState : EnemyBaseState
{

    public Animator animator;
    public float stunDuration;
    public float stunTimer;
    public ParticleSystem particle;

    public EnemyStunState(float _stunDuration) {
        stunDuration = _stunDuration;
    }

    public override void EnterState(EnemyStateManager enemy)
    {
        stunTimer = 0;
        //Debug.Log("Enter Stun State");
        enemy.particle.Play();
        enemy.animator.speed = 0;
        enemy.render.material.color = Color.red;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        stunTimer += Time.deltaTime;
        if(stunTimer >= stunDuration) {
            enemy.animator.speed = 1;
            enemy.Idle();
        }
        //Debug.Log("Enter Stun Update");
        // no longer moving
        enemy.StopPosition();
        Debug.Log("stunned");
        // after amount of time no longer stunned
      

    }
}
