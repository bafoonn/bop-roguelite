using Pasta;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// This requires: (PlayerCloseSensor script attached to player, WeaponParent script attached to enemys weapon parent, AgentMover, Detector Scripts, Steering scripts, AIData, AgentAnimations, Health, EnemyDeath scripts attached to gameobject.)

public class RangedAI : FixedEnemyAI
{
    //#region supportenemy stuff
    //[Header("Support enemy variables")]
    //public Transform supportEnemyTarger;
    //[SerializeField] private LayerMask layermask;
    //public float radius = 20;
    //private bool canTarget = true;
    //#endregion
    // Ability-related variables


    // Enemy state variables and diffrent enemy "logic"
    public bool Chasing = false;

    [Header("Animations & Speed")]
    public bool gotAttackToken = false;
    private bool DosentSeePlayer;

    protected override void Update()
    {
        if ((player.transform.position - transform.position).magnitude > 7.0f || aiData.currentTarget == null) // Deactivates attack indicator if player is not seen or is far enough away
        {
            if (attackplaceholderindicator != null)
            {
                attackplaceholderindicator.enabled = false;
            }

        }

        canAttack = true;
      

        //if(aiData.currentTarget != null)
        //{
        //    movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
        //}

        if (aiData.currentTarget != null)
        {          
            StartAttack();
        }

        if (aiData.currentTarget != null)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0)
            {
                canuseAbility = true;
            }

            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
            //Looking at target.
            PointerEnemy?.Invoke(aiData.currentTarget.position);

            if (Chasing == false)
            {
                Chasing = true;
            }

            if (aiData.currentTarget != null)
            {
                canAttack = true;
                if (!canAttack) // Ranged enemy
                {
                    SeekBehaviour seekbehaviour = gameObject.GetComponentInChildren<SeekBehaviour>();
                    seekbehaviour.targetReachedThershold = 3f;
                    attackDistance = dontattackdist;
                    detectionDelay = defaultDetectionDelay;
                    float safeDistance = 3f;
                    if (distance < safeDistance && shouldMaintainDistance)
                    {
                        movementInput = Vector2.zero;
                    }
                }

                if (distance < attackDistance)
                {
                    if (weaponParent.Scoot) // Ranged enemy "runs" away from player
                    {
                        if ((player.transform.position - transform.position).magnitude < 5.0f) // if player is in range of enemy do this.
                        {
                            SeekBehaviour seekbehaviour = gameObject.GetComponentInChildren<SeekBehaviour>();
                            seekbehaviour.targetReachedThershold = 10f;
                            Vector3 direction = transform.position - player.transform.position;
                            //direction.y = 0;
                            direction = Vector3.Normalize(direction);
                            transform.rotation = Quaternion.Euler(direction);
                            movementInput = direction;
                        }
                        else
                        {                           
                            weaponParent.Scoot = false;
                        }
                    }
                    //attackIndicator.enabled = true;

                    if (timeToAttack >= defaultTimeToAttack / 1.5) // Stops enemy from aiming when close to attacking.
                    {
                        weaponParent.Aim = false;
                        animations.aim = false;
                    }
                    //attackIndicator.fillAmount = timeToAttack / defaultTimeToAttack;
                    if (targetDetector.colliders == null)
                    {
                        SeekBehaviour seekbehaviour = gameObject.GetComponentInChildren<SeekBehaviour>();
                        seekbehaviour.targetReachedThershold = 5f;
                        seekbehaviour.targetPositionCached = player.transform.position;
                        movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                        aiData.currentTarget = null;
                    }
                }
                if (distance > attackDistance + 0.5f) // TEMP SOLUTION 
                {
                    if (hasAttackEffect) attackEffect.CancelAttack();
                }
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            //Getting the target
            aiData.currentTarget = aiData.targets[0];
        }

        #region Attacking // TODO: Test this more (added here since seemed to bug out after putting to attack state)
        if (isAttacking == true) // If is attacking start increasing timetoattack and when that reaches defaulttimetoattack then attack
        {
            timeToAttack += Time.deltaTime;
        }
        if (timeToAttack >= defaultTimeToAttack - 0.1f) // Attack indicator stuff // Added timetoattack reset to chasing and idle states so that if player runs away it resets
        {
            Attack(); // Attack method
            timeToAttack = 0;
            isAttacking = false;
            detectionDelay = defaultDetectionDelay;
        }
        if (timeToAttack >= defaultTimeToAttack / 1.5) // Stops enemy from aiming when close to attacking.
        {
            weaponParent.Aim = false;
            animations.aim = false;
        }
        if (canAttack)
        {
            if (attackplaceholderindicator != null)
            {
                attackplaceholderindicator.enabled = true;
            }
        }
        #endregion

        OnMovementInput?.Invoke(movementInput);
    }

    public override void Attack()
    {

        weaponParent.RangedAttack();
        if (hasAttackEffect) attackEffect.CancelAttack(); // Stops indicator
        
        //if (gameObject.name.Contains("Support"))
        //{
        //    weaponParent.ImmunityBeam();
        //    canAttack = false;
        //}

    }

    public void UseAbility()
    {
        if (abilityHolder.ability != null)
        {

            if (abilityHolder.ability.usableOutsideAttackRange == true)
            {

                if (abilityHolder.ability.randomize) // If ability has randomize bool true execute.
                {

                    RandomInt = Random.Range(1, 8);
                    if (RandomInt == 1 && canuseAbility)
                    {
                        canuseAbility = false;
                        movementInput = Vector2.zero;
                        abilityHolder.UseAbility = true;
                        canAttackAnim = false;
                        if (hasAttackEffect) attackEffect.CancelAttack();
                        StartCoroutine(CanAttack());
                    }
                    else
                    {
                        canuseAbility = false;
                        if (hasAttackEffect) attackEffect.CancelAttack();
                        StartCoroutine(CanAttack());
                    }
                }
                else // If not just do normally
                {
                    movementInput = Vector2.zero;
                    abilityHolder.UseAbility = true;
                    canAttackAnim = false;
                    if (hasAttackEffect) attackEffect.CancelAttack();
                    StartCoroutine(CanAttack());
                }

            }
        }

    }

    public override void ActivateIndicator() // Called from PlayerCloseSensor script when getting attack token. // DELETE THIS
    {
        base.ActivateIndicator();
        //if (hasAttackEffect) attackEffect.SetIndicatorLifetime(1f);
        //if (hasAttackEffect) attackEffect.AttackIndicator();
    }

    public override void DeActivateIndicator()
    {
        base.DeActivateIndicator();
        if (hasAttackEffect) attackEffect.SetIndicatorLifetime(0);
        if (hasAttackEffect) attackEffect.CancelAttack();
    }

    public override IEnumerator AttackCourotine()
    {
        IsIdle = false;
        isAttacking = true; // FOR ANIMATOR
                            //Attacking 
        if (abilityHolder.ability != null) abilityHolder.CanUseAbility = true;
        movementInput = Vector2.zero;
        OnAttackPressed?.Invoke();
        attackDistance = attackStopDistance;
        yield return new WaitForSeconds(defaultTimeToAttack);
        isAttacking = false;
        canAttack = false;
        stopAttacking = false;

    }
}
