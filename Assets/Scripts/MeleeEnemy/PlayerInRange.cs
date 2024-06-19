using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRange : MonoBehaviour
{
    public bool playerHit;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            playerHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerHit = false;
        }
    }
}
