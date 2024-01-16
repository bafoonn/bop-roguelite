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
        private bool returnThis;
        private bool attacking;

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

            SeekBehaviour seekbehaviour = parent.gameObject.GetComponentInChildren<SeekBehaviour>();
            seekbehaviour.targetReachedThershold = 1f; // This is default 0.5f
            enemyAI.shouldMaintainDistance = false;
            enemyAI.attackDistance = enemyAI.attackDefaultDist;
            enemyAI.detectionDelay = 0.1f;
            if (enemyAI.attackplaceholderindicator != null)
            {
                enemyAI.attackplaceholderindicator.enabled = true;
            }

            enemyAI.gotAttackToken = true;

            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

            if (enemyAI.canAttack)
            {
                
                enemyAI.movementInput = enemyAI.movementDirectionSolver.GetDirectionToMove(enemyAI.steeringBehaviours, aiData);
                if (distance < enemyAI.attackDistance && enemyAI.canAttack && enemyAI.canAttackAnim)  // if distance is smaller than attackdistance execute attack.
                {
                    Debug.Log("Starting attack");
                    enemyAI.StartAttack();
                    enemyAI.movementInput = Vector2.zero;
                    while (enemyAI.canAttack)
                    {
                        enemyAI.OnAttackPressed?.Invoke();
                        enemyAI.isAttacking = true;
                        if (enemyAI.timeToAttack >= enemyAI.defaultTimeToAttack) // Attack indicator stuff // Added timetoattack reset to chasing and idle states so that if player runs away it resets
                        {
                            Debug.Log("Attacking");
                            enemyAI.Attack(); // Attack method
                            enemyAI.timeToAttack = 0;
                            enemyAI.isAttacking = false;
                            enemyAI.detectionDelay = enemyAI.defaultDetectionDelay;
                            enemyAI.canAttack = false;
                        }
                    }
                }
                return this;
            }
            else
            {
                return idleState;
            }

        }

        

        
    }
}
