using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        //Debug.Log("Enter Stun State");
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        //Debug.Log("Enter Stun Update");
        // no longer moving
        enemy.StopPosition();
        Debug.Log("stunned");
        // after amount of time no longer stunned
        enemy.IsStunned();
    }
}