using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    // all the states of the enemies
    public EnemyBaseState currentState;
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyAggroState aggroState = new EnemyAggroState();
    public EnemyAttackState attackState = new EnemyAttackState();
    public EnemyDeathState deathState = new EnemyDeathState();

    // player transform
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        // starting state for the state machine, aka idle
        currentState = idleState;
        // "this" is a ref to the context (this EXACT monobehavior script)
        // will call logic from EnterState
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {    
        // will call any logic in UpdateState from the current state every frame
        currentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState state)
    {
        // change the state then call the EnterState from the new state
        currentState = state;
        state.EnterState(this);
    }

    public bool RayCastCheck(float distance)
    {
        if (Physics.Raycast(transform.position, player.transform.position, out RaycastHit hitInfo, distance))
        {
            // draws the ray in scene when hit, RED
            Debug.DrawRay(transform.position, player.transform.position * hitInfo.distance, Color.red);
            return true;
        }
        else
        {
            // draws the ray in scene when NOT hit, GREEN
            Debug.DrawRay(transform.position, player.transform.position * distance, Color.green);
            return false;
        }
    }
}
