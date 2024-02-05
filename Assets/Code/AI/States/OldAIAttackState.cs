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
        private Transform player;

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

            SeekBehaviour seekbehaviour = parent.gameObject.GetComponentInChildren<SeekBehaviour>();
            seekbehaviour.targetReachedThershold = enemyAI.attackDefaultDist; // This is default 0.5f
            enemyAI.shouldMaintainDistance = false;
            //enemyAI.attackDistance = enemyAI.attackDefaultDist;
            enemyAI.detectionDelay = 0.1f;

            
            //float distance = Vector2.Distance(player.position, parent.transform.position);

            if (enemyAI.canAttack)
            {
                enemyAI.movementInput = enemyAI.movementDirectionSolver.GetDirectionToMove(enemyAI.steeringBehaviours, aiData);
                if ((player.transform.position - parent.transform.position).magnitude < enemyAI.attackDistance)
                {
                    enemyAI.StartAttack();
                    if (enemyAI.timeToAttack >= enemyAI.defaultTimeToAttack - 0.1f) // Attack indicator stuff // Added timetoattack reset to chasing and idle states so that if player runs away it resets
                    {
                        enemyAI.Attack(); // Attack method
                        enemyAI.timeToAttack = 0;
                        enemyAI.isAttacking = false;
                        enemyAI.detectionDelay = enemyAI.defaultDetectionDelay;
                    }
                    if (enemyAI.timeToAttack >= enemyAI.defaultTimeToAttack / 1.5) // Stops enemy from aiming when close to attacking.
                    {
                        enemyAI.weaponParent.Aim = false;
                        enemyAI.animations.aim = false;
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
