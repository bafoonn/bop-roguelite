using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
	public class OldAIApproachState : State
	{
		public OldAIAttackState attackState;
		public bool gotAttackToken;
		private FixedEnemyAI enemyAI;
		private TargetDetector targetDetector;
		private AIData aiData;
		private Transform parent;
		private bool CanAttack;
		public override State EnterState()
		{
			Debug.Log("Entered approach state");
			parent = transform.parent.transform.parent;
			enemyAI = parent.GetComponent<FixedEnemyAI>();
			targetDetector = parent.GetComponentInChildren<TargetDetector>();
			aiData = parent.GetComponent<AIData>();
			return this;
		}

		public override State RunCurrentState()
		{
			parent = transform.parent.transform.parent;
			enemyAI = parent.GetComponent<FixedEnemyAI>();
			targetDetector = parent.GetComponentInChildren<TargetDetector>();
			aiData = parent.GetComponent<AIData>();

			CanAttack = enemyAI.canAttack;
			float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
			if (!CanAttack)
			{
				// This puts the distance enemies will stay away from player if hasnt gotten attack token
				SeekBehaviour seekbehaviour = parent.gameObject.GetComponentInChildren<SeekBehaviour>();
				seekbehaviour.targetReachedThershold = 3f; // Just here to stop seekbehaviour from reaching target too soon and stopping.
				enemyAI.attackDistance = enemyAI.dontattackdist; // decrease attack dist
				float safeDistance = 5f;
				if (distance < safeDistance && enemyAI.shouldMaintainDistance) // If inside safedistance stop moving & getting shouldMaintainDistance bool from PlayerCloseSensor which stops enemys from all attacking at the same time.
				{
					enemyAI.movementInput = Vector2.zero;
				}
				return this;
			}
			else
			{
				SeekBehaviour seekbehaviour = parent.gameObject.GetComponentInChildren<SeekBehaviour>();
				seekbehaviour.targetReachedThershold = 1f; // This is default 0.5f
				enemyAI.shouldMaintainDistance = false;
				enemyAI.attackDistance = enemyAI.attackDefaultDist;
				enemyAI.detectionDelay = 0.1f;
				return attackState;
			}
			
		}
	}
}
