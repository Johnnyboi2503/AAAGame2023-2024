using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttackState : MeleeEnemyBaseState
{
    float attackDistance;

    public Animator animator;

    public MeleeEnemyAttackState(float _attackDistance) {
        attackDistance = _attackDistance;
    }
    public override void EnterState(MeleeEnemyStateManager enemy)
    {
        //Debug.Log("Enter Attack State");
        enemy.StopPosition();
        


    }

    public override void UpdateState(MeleeEnemyStateManager enemy)
    {
        // NOT DONE
        // melee attack?
        enemy.animator.SetBool("isAttacking", true);
        if(enemy.DetectPlayer())
            enemy.MeleeAttack();
        else
            enemy.LookAtPlayer();


        //Debug.Log("Enter Attack Update");
        // if enemy is out of range for attack
        if (!(enemy.RayCastCheck(attackDistance))) {
            enemy.SwitchState(enemy.aggroState);
            enemy.animator.SetBool("isAttacking", false);
        }
    }
}
