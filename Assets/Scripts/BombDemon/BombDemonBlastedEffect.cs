using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDemonBlastedEffect : EnergyBlastedEffect
{
    public BombDemon bombDemon;
    [SerializeField] float deathSpeedIncrease; // The percent boost given to the player when killed

    public override void TriggerEffect()
    {
        if (bombDemon.state != BombDemon.State.dead)
        {
            FindObjectOfType<MovementModification>().AddSpeedBoost(float.MaxValue, deathSpeedIncrease); // Adding speed boost to player when dead
            bombDemon.Exploding();
        }
    }

   

}
