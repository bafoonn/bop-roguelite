using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class NewEnemy : MonoBehaviour
    {
		public Transform target;
		public float speed = 10f;
		public float turnSpeed = 3;
		public float turnDist = 0;
		private Rigidbody2D rbd2d;
		public float stoppingDist = 10f;
		Vector3 moveDirection;
		const float updateMovementThreshold = .5f;
		const float minPathUpdateTime = .2f;
		[SerializeField] private float targetDetectionRange = 2;
		[SerializeField] private float targetDetectionRangeGizmo = 10;
		[SerializeField] private LayerMask playerLayerMask;
		public bool followingPath = true;
		[SerializeField] private LayerMask EnemyLayerMask; //TODO: Put these in states
		public bool FinishedPath = false;
		public float radius = 1; //TODO: Put these in states
		//Vector3[] path;
		//int targetIndex;

		Path path;

		private void Start()
		{
			//target = GameObject.FindGameObjectWithTag("Player").transform;
			//StartCoroutine(UpdatePath());
			rbd2d = GetComponent<Rigidbody2D>();
		}


		public void OnPathFound(Vector3[] waypoints, bool pathSuccess)
		{
			if (pathSuccess)
			{
				path = new Path(waypoints, transform.position, turnDist , stoppingDist);
				StopCoroutine("FollowPath");
				StartCoroutine("FollowPath");
			}
		}

		private void Update()
		{
			if ((target.transform.position - transform.position).magnitude < 4.0f)
			{
				FinishedPath = true;
				followingPath = false;
				Debug.Log("Finished path");
			}
		}

		public void StartPath()
		{
			followingPath = true;			
			StartCoroutine(UpdatePath());
		}

		public void StopFollowingPath()
		{
			StopCoroutine("FollowPath");
			StartCoroutine("Continue");
		}

		IEnumerator Continue()
		{
			yield return new WaitForSeconds(1f);
			StartPath();
		}

		IEnumerator UpdatePath()
		{
			if(Time.timeSinceLevelLoad < .3f)
			{
				yield return new WaitForSeconds(.3f);
			}
			PathRequestManager.RequestPath(new pathRequest(transform.position, target.position, OnPathFound));
			float sqrMoveThreshold = updateMovementThreshold * updateMovementThreshold;
			Vector3 targetOldPos = target.position;
			while (true)
			{
				yield return new WaitForSeconds(minPathUpdateTime);
				if((target.position - targetOldPos).sqrMagnitude > sqrMoveThreshold)
				{
					PathRequestManager.RequestPath(new pathRequest(transform.position, target.position, OnPathFound));
					targetOldPos = target.position;
				}
				
			}
		}


		IEnumerator FollowPath()
		{
			//Vector3 currentWaypoint = path[0];
			
			int pathIndex = 0;
			FinishedPath = false;
			//transform.LookAt(path.lookPoints[0]);

			while (followingPath)
			{
				Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
				if (path.turnBoundaries[pathIndex].hasCrossedLine(pos2D))
				{
					if(pathIndex == path.finishLineIndex)
					{
						followingPath = false;
						break;
					}
					else
					{
						
						moveDirection = (path.lookPoints[pathIndex] - transform.position).normalized;
						pathIndex++;				
					}
				}

				if (followingPath)
				{
					Vector3 targetDirection = (path.lookPoints[pathIndex] - transform.position).normalized;
					Debug.DrawLine(transform.position, transform.position + targetDirection);
					moveDirection = Vector3.Lerp(moveDirection, targetDirection, Time.deltaTime * turnSpeed).normalized;
					//transform.Traslate(moveDirection * Time.deltaTime * speed);
					transform.position += (moveDirection * Time.deltaTime * speed);
				}
				yield return null;
			}
		}

		public void OnDrawGizmos()
		{
			if(path != null)
			{
				//for(int i = targetIndex; i < path.Length; i++)
				//{
				//	Gizmos.color = Color.black;
				//	Gizmos.DrawCube(path[i], Vector3.one);
				//	if(i == targetIndex)
				//	{
				//		Gizmos.DrawLine(transform.position, path[i]);
				//	}
				//	else
				//	{
				//		Gizmos.DrawLine(path[i - 1], path[i]);
				//	}
				//}
				path.DrawWithGizmos();
				
				
				
			}
			Gizmos.DrawWireSphere(transform.position, targetDetectionRangeGizmo);
		}


	}

	//Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, EnemyLayerMask); //TODO: Put these in states
	//for (int i = 0; i < hitColliders.Length; i++)
	//{
	//	if (hitColliders[i].gameObject.TryGetComponent(out NewEnemy newEnemyAI))
	//	{
	//		if(i == 0)
	//		{
	//			continue;
	//		}
	//		else
	//		{
	//			newEnemyAI = hitColliders[i].gameObject.GetComponent<NewEnemy>();
	//			newEnemyAI.StopFollowingPath();
	//		}
	//	}
	//} //TODO: Put these in states
}
