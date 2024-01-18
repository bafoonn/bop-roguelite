using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
	public class OldAIIdleState : State
	{
		private bool canSeePlayer;
		public OldAIChaseState oldchaseState;
		public GameObject Enemy;
		private FixedEnemyAI enemyAI;
		public TargetDetector targetDetector;
		public AIData aiData;
		private Transform parent;
		public override State EnterState()
		{
			parent = transform.parent.transform.parent;
			Enemy = transform.parent.gameObject;
			enemyAI = parent.GetComponent<FixedEnemyAI>();
			targetDetector = parent.GetComponentInChildren<TargetDetector>();
			return this;
		}

		public override State RunCurrentState()
		{
            parent = transform.parent.transform.parent;
            Enemy = transform.parent.gameObject;
            enemyAI = parent.GetComponent<FixedEnemyAI>();
            targetDetector = parent.GetComponentInChildren<TargetDetector>();

            if (aiData.currentTarget == null) // If current target is null go to idle.
			{
				enemyAI.attackDistance = enemyAI.attackDefaultDist;
				if (enemyAI.hasAttackEffect) enemyAI.attackEffect.CancelAttack();
				enemyAI.isAttacking = false; // FOR ANIMATOR
				enemyAI.IsIdle = true;
				if (enemyAI.abilityHolder.ability != null) enemyAI.abilityHolder.CanUseAbility = false;
				enemyAI.timeToAttack = 0;
				
			}
			if (targetDetector.SeenPlayer)
			{
				canSeePlayer = true;
			}
			else
			{
				canSeePlayer = false;
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
