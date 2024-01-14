using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
	public class OldAIApproachState : State
	{
		public OldAIAttackState attackState;
		public bool canAttack;
		private GameObject Enemy;
		private EnemyAi enemyAI;
		private TargetDetector targetDetector;
		private AIData aiData;
		public override State EnterState()
		{
			Enemy = transform.parent.gameObject;
			enemyAI = GetComponentInParent<EnemyAi>();
			targetDetector = Enemy.GetComponentInChildren<TargetDetector>();
			aiData = Enemy.GetComponentInChildren<AIData>();
			return this;
		}

		public override State RunCurrentState()
		{
			float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
			if (!canAttack)
			{
				// This puts the distance enemies will stay away from player if hasnt gotten attack token
				SeekBehaviour seekbehaviour = Enemy.gameObject.GetComponentInChildren<SeekBehaviour>();
				seekbehaviour.targetReachedThershold = 2f; // Just here to stop seekbehaviour from reaching target too soon and stopping.
				//enemyAI.attackDistance = enemyAI.dontattackdist; // decrease attack dist
				//enemyAI.detectionDelay = enemyAI.defaultDetectionDelay; // How frequently "enemy" updates eg. looks for player.
				float safeDistance = 5f;
				if (distance < safeDistance && enemyAI.shouldMaintainDistance) // If inside safedistance stop moving & getting shouldMaintainDistance bool from PlayerCloseSensor which stops enemys from all attacking at the same time.
				{
					enemyAI.movementInput = Vector2.zero;
				}
				return this;
			}
			else
			{
				SeekBehaviour seekbehaviour = Enemy.gameObject.GetComponentInChildren<SeekBehaviour>();
				seekbehaviour.targetReachedThershold = 1f; // This is default 0.5f
				enemyAI.shouldMaintainDistance = false;
				enemyAI.attackDistance = enemyAI.attackDefaultDist;
				enemyAI.detectionDelay = 0.1f;
				return attackState;
			}
			
		}
	}
}
