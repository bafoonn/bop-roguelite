using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
	public class PatrolState : State
	{
		public ChaseState chaseState;
		public bool canSeePlayer;
		private GameObject[] waypoints;
		private Transform waypoint;
		private NewEnemy EnemyScript;
		private int random = 0;
		[SerializeField] private float targetDetectionRange = 5;
		[SerializeField] private LayerMask obstacleLayerMask, playerLayerMask;
		private bool Done = false;

		public override State EnterState()
		{
			if(Done != true)
			{
				EnemyScript = GetComponentInParent<NewEnemy>();
				waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
				random = UnityEngine.Random.Range(0, waypoints.Length);
				waypoint = waypoints[random].transform;
				EnemyScript.target = waypoint;
				EnemyScript.StartPath();
				Done = true;
			}
			
			return this;
		}

		public override State RunCurrentState()
		{
			Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, targetDetectionRange, playerLayerMask); // TODO: Put into state
			if (playerCollider != null)
			{
				Vector2 rayDir = (playerCollider.transform.position - transform.position).normalized;
				RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, targetDetectionRange, obstacleLayerMask);
				if (hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
				{
					canSeePlayer = true;
				}
			}
			if (canSeePlayer)
			{
				return chaseState;
			}
			else
			{
				return this;
			}
			
			
		}
	}
}
