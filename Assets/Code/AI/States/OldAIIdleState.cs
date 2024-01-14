using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
	public class OldAIIdleState : State
	{
		private bool canSeePlayer;
		public OldAIChaseState oldchaseState;
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
			if (aiData.currentTarget == null) // If current target is null go to idle.
			{
				enemyAI.attackDistance = enemyAI.attackDefaultDist;

				if (enemyAI.hasAttackEffect) enemyAI.attackEffect.CancelAttack();

				enemyAI.isAttacking = false; // FOR ANIMATOR

				enemyAI.IsIdle = true;

				if (enemyAI.abilityHolder.ability != null) enemyAI.abilityHolder.CanUseAbility = false;

				enemyAI.timeToAttack = 0;

				enemyAI.Chasing = false;

			}
			if (targetDetector.SeenPlayer)
			{
				canSeePlayer = true;
			}
			if (canSeePlayer)
			{
				return oldchaseState;
			}
			else
			{
				return this;
			}
		}
	}
}
