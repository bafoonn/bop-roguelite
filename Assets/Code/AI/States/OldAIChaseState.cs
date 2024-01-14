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
			enemyAI.attackDistance = enemyAI.attackDefaultDist;

			enemyAI.weaponParent.Aim = true;

			enemyAI.animations.aim = true;

			enemyAI.isAttacking = false; // FOR ANIMATOR
								 //Chasing

			if (enemyAI.abilityHolder.ability != null) enemyAI.abilityHolder.CanUseAbility = true; // <- Here for testing purposes.

			if (enemyAI.hasAttackEffect) enemyAI.attackEffect.SetIndicatorLifetime(0);

			if (enemyAI.hasAttackEffect) enemyAI.attackEffect.CancelAttack();

			enemyAI.UseAbility();

			enemyAI.timeToAttack = 0;

			enemyAI.movementInput = enemyAI.movementDirectionSolver.GetDirectionToMove(enemyAI.steeringBehaviours, aiData);

			if (isInAttackRange)
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
