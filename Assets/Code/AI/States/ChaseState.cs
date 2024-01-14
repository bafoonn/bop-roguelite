using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
	public class ChaseState : State
	{
		public ApproachPlayerState approachPlayer;
		private NewEnemy EnemyScript;
		private Transform player;
		public override State EnterState()
		{
			player = GameObject.FindGameObjectWithTag("Player").transform;
			EnemyScript = GetComponentInParent<NewEnemy>();
			EnemyScript.target = player;
			//EnemyScript.StopFollowingPath();
			return this;
		}

		public override State RunCurrentState()
		{
			
			return this;
		}
	}
}
