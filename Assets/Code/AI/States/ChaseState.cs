using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
	public class ChaseState : State
	{
		public ApproachPlayerState approachPlayer;
		
		public PatrolState patrolState;
		public bool hasGottenToken = false;
		private NewEnemy EnemyScript;
		private Transform player;
		public bool closeToPlayer;
		private Transform parent;
		public override State EnterState()
		{
			player = GameObject.FindGameObjectWithTag("Player").transform;
			parent = transform.parent.transform.parent;
			EnemyScript = parent.GetComponent<NewEnemy>();
			EnemyScript.target = player;
			//EnemyScript.StopFollowingPath();
			return this;
		}

		public override State RunCurrentState()
		{
			if ((player.transform.position - transform.position).magnitude < 4.5f)
			{
				Debug.Log("Close to player");
				closeToPlayer = true;
			}
			else
			{
				closeToPlayer = false;
			}
			if (!EnemyScript.followingPath && !closeToPlayer && !hasGottenToken)
			{
				return patrolState;
			}
			if (closeToPlayer && hasGottenToken)
			{
				Debug.Log("Got attack token new enemy");
				return approachPlayer;
			}
			else
			{
				return this;
			}
			
		}
	}
}
