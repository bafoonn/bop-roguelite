using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
	public class OldAIChaseState : State
	{
		public bool isInAttackRange;
		public OldAIApproachState approachState;
		private GameObject Enemy;
		private FixedEnemyAI enemyAI;
		private TargetDetector targetDetector;
		private AIData aiData;
		private Transform player;
		private Transform parent;

		public bool closeToPlayer;
		public override State EnterState()
		{
			Debug.Log("Entered chase state");
			player = GameObject.FindGameObjectWithTag("Player").transform;
			parent = transform.parent.transform.parent;
			Enemy = parent.gameObject;
			enemyAI = parent.GetComponent<FixedEnemyAI>();
			targetDetector = parent.GetComponentInChildren<TargetDetector>();
			aiData = parent.GetComponentInChildren<AIData>();
			return this;
		}

		public override State RunCurrentState()
		{
			parent = transform.parent.transform.parent;
			enemyAI = parent.GetComponent<FixedEnemyAI>();
			targetDetector = parent.GetComponentInChildren<TargetDetector>();
			aiData = parent.GetComponent<AIData>();

            SeekBehaviour seekbehaviour = parent.gameObject.GetComponentInChildren<SeekBehaviour>();
            seekbehaviour.targetReachedThershold = 1f; // This is default 0.5f

            enemyAI.attackDistance = enemyAI.attackDefaultDist;
			enemyAI.weaponParent.Aim = true;
			enemyAI.animations.aim = true;
			enemyAI.isAttacking = false; // FOR ANIMATOR
								 //Chasing

			if (enemyAI.abilityHolder.ability != null) enemyAI.abilityHolder.CanUseAbility = true; // <- Here for testing purposes.

			if (enemyAI.hasAttackEffect) enemyAI.attackEffect.SetIndicatorLifetime(0);

			if (enemyAI.hasAttackEffect) enemyAI.attackEffect.CancelAttack();

			enemyAI.UseAbilityAtRange();

			enemyAI.timeToAttack = 0;
			enemyAI.movementInput = enemyAI.movementDirectionSolver.GetDirectionToMove(enemyAI.steeringBehaviours, aiData);

			if ((player.transform.position - transform.position).magnitude < 5.5f)
			{
				Debug.Log("Close to player");
				enemyAI.movementInput = Vector2.zero;
				closeToPlayer = true;
			}
			else
			{
				closeToPlayer = false;
			}

			if (closeToPlayer)
			{
				return approachState;
			}
			else
			{
				return this;
			}
		}
	}
}
