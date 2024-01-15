using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class OldAIAttackState : State
    {
        public OldAIIdleState idleState;
        public OldAIApproachState approachState;
        private bool hasAttacked;
        private FixedEnemyAI enemyAI;
        private TargetDetector targetDetector;
        private AIData aiData;
        private Transform parent;
        private bool CanAttack;

        public override State EnterState()
		{
            Debug.Log("Entered attack state");
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

            enemyAI.IsIdle = false;
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

            // start attack at distance 
            if (!enemyAI.canAttack)
            {
                enemyAI.attackDistance = enemyAI.dontattackdist;
                if (enemyAI.attackplaceholderindicator != null)
                {
                    enemyAI.attackplaceholderindicator.enabled = false;
                }
                return approachState;
            }
            else
            {

                if (enemyAI.attackplaceholderindicator != null)
                {
                    enemyAI.attackplaceholderindicator.enabled = true;
                }

                enemyAI.gotAttackToken = true;
                enemyAI.attackDistance = enemyAI.attackDefaultDist;
            }
            // Attack state
            if (distance < enemyAI.attackDistance && enemyAI.canAttack && enemyAI.canAttackAnim)  // if distance is smaller than attackdistance execute attack.
            {
                enemyAI.isAttacking = true; // FOR ANIMATOR
                                            //Attacking 
                if (enemyAI.abilityHolder.ability != null) enemyAI.abilityHolder.CanUseAbility = true;

                enemyAI.movementInput = Vector2.zero;
                enemyAI.OnAttackPressed?.Invoke();

                if (enemyAI.timeToAttack >= enemyAI.defaultTimeToAttack) // Attack indicator stuff // Added timetoattack reset to chasing and idle states so that if player runs away it resets
                {
                    Debug.Log("Attacking");

                    enemyAI.Attack(); // Attack method
                    enemyAI.timeToAttack = 0;
                    enemyAI.isAttacking = false;
                    enemyAI.detectionDelay = enemyAI.defaultDetectionDelay;
                }
                enemyAI.attackDistance = enemyAI.attackStopDistance;
                //if (enemyAI.firstAttack) // TODO: FIX THIS IF TIME // Here only since indicator bugged out if enemy attacking first time don't know why
                //{
                //    enemyAI.timeToAttack = 0;
                //    yield return new WaitForSeconds(0);
                //}
                //else
                //{
                //    yield return new WaitForSeconds(enemyAI.defaultTimeToAttack);
                //}
                enemyAI.firstAttack = false;
                return approachState;
            }
			else
			{
                return idleState;
            }

        }
    }
}
