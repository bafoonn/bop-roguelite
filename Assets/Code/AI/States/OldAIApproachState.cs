using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
	public class OldAIApproachState : State
	{
		public OldAIAttackState attackState;
		public OldAIChaseState chaseState;

		public bool gotAttackToken;
		private FixedEnemyAI enemyAI;
		private TargetDetector targetDetector;
		private AIData aiData;
		private Transform parent;
		private bool CanAttack;
        private Transform player;

        public float backwardSpeed = 3f; // Adjust this value to control the speed of the backward movement
        private bool isTakingStepsBack = false;
        public override State EnterState() // This is currently non needed since RunCurrent State happens first need to fix this.
		{
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
            player = GameObject.FindGameObjectWithTag("Player").transform;

			
			if ((player.transform.position - parent.transform.position).magnitude > 7.5f)
			{
				return chaseState;
			}
			

			

			CanAttack = enemyAI.canAttack;
			float distance = Vector2.Distance(player.position, parent.transform.position);
			if (!CanAttack)
			{
				// This puts the distance enemies will stay away from player if hasnt gotten attack token
				SeekBehaviour seekbehaviour = parent.gameObject.GetComponentInChildren<SeekBehaviour>();
				seekbehaviour.targetReachedThershold = 4f; // Just here to stop seekbehaviour from reaching target too soon and stopping.
				enemyAI.attackDistance = enemyAI.dontattackdist; // decrease attack dist
				float safeDistance = 5f;
				if (distance < safeDistance && enemyAI.shouldMaintainDistance) // If inside safedistance stop moving & getting shouldMaintainDistance bool from PlayerCloseSensor which stops enemys from all attacking at the same time.
				{
					enemyAI.movementInput = Vector2.zero;
				}
                enemyAI.attackDistance = enemyAI.dontattackdist;
                return chaseState;
			}
			else
			{
				SeekBehaviour seekbehaviour = parent.gameObject.GetComponentInChildren<SeekBehaviour>();
				seekbehaviour.targetReachedThershold = 1f; // This is default 0.5f
				enemyAI.shouldMaintainDistance = false;
				enemyAI.attackDistance = enemyAI.attackDefaultDist;
				enemyAI.detectionDelay = 0.1f;
                enemyAI.attackDistance = enemyAI.attackDefaultDist;
                return attackState;
			}
            

        }

		
    }
}
