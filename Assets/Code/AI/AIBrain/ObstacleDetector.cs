using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : Detector
{
    [SerializeField] private float detectionRadius = 2; // Detect obstacles in x radius, so it dosent detect all object in the scene
    [SerializeField] private LayerMask layerMask; // Obstacles layermask
    [SerializeField] private bool showGizmos = true; // Change this to true when done testing.
    Collider2D[] colliders; // Refs to detected obstacles


    public override void Detect(AIData aiData)
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, layerMask); // Detect all obstacles in the circle
        aiData.obstacles = colliders; // Add detected obstacles to aidata.obstacles
    }

    private void OnDrawGizmos()
    {
        if (showGizmos == false)
        {
            return;
        }
        if(Application.isPlaying && colliders != null)
        {
            Gizmos.color = Color.red;
            foreach(Collider2D obstacleCol in colliders)
            {
                Gizmos.DrawSphere(obstacleCol.transform.position, 0.2f);
            }
        }
    }
}
