using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSword : MonoBehaviour
{
    public float damage;
    public bool hitPlayer = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out BloodThirst blood) && !hitPlayer)
        {
            blood.LoseBlood(damage, this.gameObject);
            hitPlayer = true;
        }
    }
}
