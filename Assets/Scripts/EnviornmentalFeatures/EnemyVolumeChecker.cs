using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVolumeChecker : MonoBehaviour
{
    //this script simply checks periodically if there are any enemies inside the box collider of the object
    //You can adjust the volume by changing the box collider size
    //You can adjust the check interval by changing the checkInterval variable

    public bool enemiesInside = true;
    [SerializeField] float checkInterval = 1f;
    BoxCollider boxCollider;

    //assign this if you're using this script to open a barrier once all enemies inside are killed
    //otherwise you can leave this empty
    [SerializeField]Barrier barrier;


    void Start(){
        boxCollider = GetComponent<BoxCollider>();
        if(boxCollider == null){
            Debug.LogError("No BoxCollider found");
            return;
        }
        CheckEnemiesInside();
        StartCoroutine(PeriodicEnemyCheck());
    }

    IEnumerator PeriodicEnemyCheck(){
        while(true){
            yield return new WaitForSeconds(checkInterval);
            CheckEnemiesInside();
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            enemiesInside = true;
        }
    }
    void OnTriggerExit(Collider other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            CheckEnemiesInside();
        }
    }

    private void CheckEnemiesInside(){
        if(boxCollider == null){
            Debug.LogError("No BoxCollider found");
            return;
        }

        Vector3 center = boxCollider.bounds.center;
        Vector3 halfExtents = boxCollider.bounds.extents;

        Collider[] colliders = Physics.OverlapBox(transform.position, halfExtents, Quaternion.identity, LayerMask.GetMask("Enemy"));
        if(colliders.Length > 0){
            enemiesInside = true;
            Debug.Log("Enemies inside");
            }   
        else {
            enemiesInside = false;
            AllEnemiesKilled();
            Debug.Log("No enemies inside");
        }
    }



    void AllEnemiesKilled(){
        if(!enemiesInside && barrier != null){
            barrier.OpenBarrier();
        }
    }
}
