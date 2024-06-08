using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    //this script is used to block the player from progressing until all enemies inside the barrier are killed
    //You can adjust the scale of the barrier by changing the scale of the object
    //Choose the type of barrier removal in the inspector, and set the speed and distance accordingly if using a moving barrier
    
    public enum BarrierRemovalType { 
        DestroyBarrier,
        MoveBarrierAlongYAxis,
        MoveBarrierAlongXAxis,
        MoveBarrierAlongZAxis
    }
    [SerializeField] BarrierRemovalType barrierRemovalType;
    [SerializeField] float barrierSpeed = 1f;
    [SerializeField] float barrierDistance = 10f;
    bool barrierHasBeenOpened = false;
    [SerializeField] AnimationCurve movementCurve;

    public void OpenBarrier(){ 
        //assuming a barrier can only be opened once
        if(barrierHasBeenOpened) return;

        Debug.Log("Barrier Opened");
        barrierHasBeenOpened = true;
        switch(barrierRemovalType){
            case BarrierRemovalType.DestroyBarrier:
                Destroy(gameObject);
                break;
            case BarrierRemovalType.MoveBarrierAlongYAxis:
                StartCoroutine(MoveBarrier(Vector3.up));
                break;
            case BarrierRemovalType.MoveBarrierAlongXAxis:
                StartCoroutine(MoveBarrier(Vector3.right));
                break;
            case BarrierRemovalType.MoveBarrierAlongZAxis:
                StartCoroutine(MoveBarrier(Vector3.forward));
                break;
        }
        
    }

    private IEnumerator MoveBarrier(Vector3 direction)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position + direction * barrierDistance;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            float elapsedTime = Time.time - startTime;
            float fractionOfJourney = elapsedTime / (journeyLength / barrierSpeed);
            float curveValue = movementCurve.Evaluate(fractionOfJourney);
            transform.position = Vector3.Lerp(startPosition, targetPosition, curveValue);
            yield return null;
        }
    }

}
