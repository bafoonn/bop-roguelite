using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField] public float targetReachedThershold = 0.5f; //Go to last pos player was seen if lost sight
    [SerializeField] private bool ShowGizmos = true;
    bool reachedLastTarget = true;

    private Vector2 targetPositionCached;
    private float[] intrestsTemp;
    public override (float[] danger, float[] intrest) GetSteering(float[] danger, float[] intrest, AIData aiData)
    {
        //if enemy dosen't have a target stop seeking
        //else set a new target.
        if (reachedLastTarget)
        {
            if(aiData.targets == null || aiData.targets.Count <= 0)
            {
                aiData.currentTarget = null;
                return (danger, intrest);
            }
            else
            {
                // Select the target closest to the enemy
                reachedLastTarget = false;                                                                                      //FirstOrDefault to avoid null exceptions.
                aiData.currentTarget = aiData.targets.OrderBy(target => Vector2.Distance(target.position, transform.position)).FirstOrDefault();
            }
        }
        //Caches the last position if we still see the target
        if(aiData.currentTarget != null && aiData.targets != null && aiData.targets.Contains(aiData.currentTarget))
        {
            targetPositionCached = aiData.currentTarget.position;
        }
        //Check if has reached the target   
        if(Vector2.Distance(transform.position, targetPositionCached) < targetReachedThershold)
        {
            Debug.Log("Reached target");
            reachedLastTarget = true;
            aiData.currentTarget = null;
            return(danger, intrest);
        }
        //If we havent's reached the target do the main logic
        Vector2 directionToTarget = (targetPositionCached - (Vector2)transform.position); 
        for(int i = 0; i < intrest.Length; i++)
        {
            float result = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);
            //Accept only directions that are less than 90degrees
            if(result > 0) // HAD THIS AS < AND DIDINT REALISE SO WAS BUG FIXING FOR 3 DAYS.
            {
                float valuetoputin = result;
                if(valuetoputin > intrest[i])
                {
                    intrest[i] = valuetoputin;
                }
            }
        }
        intrestsTemp = intrest;
        return (danger, intrest);

    }
    private void OnDrawGizmos()
    {

        if (ShowGizmos == false)
            return;
        Gizmos.DrawSphere(targetPositionCached, 0.2f);

        if (Application.isPlaying && intrestsTemp != null)
        {
            if (intrestsTemp != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < intrestsTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * intrestsTemp[i] * 2);
                }
                if (reachedLastTarget == false)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(targetPositionCached, 0.1f);
                }
            }
        }
    }
}
