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

        public override State EnterState()
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
            enemyAI.attackDistance = enemyAI.attackDefaultDist;
            enemyAI.detectionDelay = 0.1f;

            

            

            //float distance = Vector2.Distance(player.position, parent.transform.position);

            if (enemyAI.canAttack)
            {
               

                enemyAI.movementInput = enemyAI.movementDirectionSolver.GetDirectionToMove(enemyAI.steeringBehaviours, aiData);
                if ((player.transform.position - transform.position).magnitude < enemyAI.attackDistance)
                {
                    Debug.Log("Starting attack");
                    enemyAI.StartAttack();
                    enemyAI.movementInput = Vector2.zero;
                }
                //if (distance < enemyAI.attackDistance && enemyAI.canAttack && enemyAI.canAttackAnim)  // if distance is smaller than attackdistance execute attack.
                //{
                //    Debug.Log("Starting attack");
                //    enemyAI.StartAttack();
                //    enemyAI.movementInput = Vector2.zero;
                //}
                return this;
            }
            else
            {
                return idleState;
            }

        }

        

        
    }
}
