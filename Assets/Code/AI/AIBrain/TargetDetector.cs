using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class TargetDetector : Detector
{
    //If having problems detecting;
    //You want to Disable 'Queries Start In Colliders' under Edit - Project Settings - Physics 2D 
    //This will stop the raycast from counting the enemy collider that it starts in
    [SerializeField] private float targetDetectionRange = 5; // How far does the ai detect. Circle cast.
    [SerializeField] private LayerMask obstacleLayerMask, playerLayerMask; // Check if player is visible to enemy.
    [SerializeField] private bool ShowGizmos = true;
    private Transform player;

    public List<Transform> colliders;
    public bool SeenPlayer = false;
    public override void Detect(AIData aiData)
    {
        // If player is near.
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, targetDetectionRange, playerLayerMask);

        if (playerCollider != null)
        {
           
             // If this detects but enemy dosent move then try increasing player collider size
            // Checks if enemy can see the player.
            Vector2 direction = (playerCollider.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, targetDetectionRange, obstacleLayerMask);
            Debug.DrawRay(transform.position, direction, Color.green);
            //Check if the collider hit is on the player layermask.
            if (hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Debug.DrawRay(transform.position, direction * targetDetectionRange, Color.magenta);
                colliders = new List<Transform>() { playerCollider.transform };
                SeenPlayer = true;
            }
            else
            {   // Dosen't see player.
                colliders = null;
                SeenPlayer = false;
                //if (!SeenPlayer) // TEST TO REVERT
                //{
                //    colliders = null;
                //}
            }
        }
        else
        {
            colliders = null;
            //if (!SeenPlayer) // TEST TO REVERT
            //{
            //    colliders = null;
            //}

        }
        aiData.targets = colliders;
    }

    private void OnDrawGizmos()
    {
        if (ShowGizmos == false)
        {
            return;
        }
        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);
        if (colliders == null)
        {
            return;
        }
        Gizmos.color = Color.blue;
        foreach (var item in colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }

    }



}
