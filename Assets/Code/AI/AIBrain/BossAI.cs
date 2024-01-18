using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Pasta;


public class BossAI : EnemyAi
{
    public bool indicatorAlive = false;
    public UnityEvent<Vector2> OnPointerInput;
    public float CurrentHealthPercentage;

    //[SerializeField] private float chaseDistanceThershold = 3, attackDistanceThershold = 0.8f;
    //private float passedTime = 1;

    private void Update()
    {
        CurrentHealthPercentage = (Health.CurrentHealth / Health.MaxHealth) * 100;
        if (aiData.currentTarget != null)
        {

            //Looking at target.
            OnPointerInput?.Invoke(aiData.currentTarget.position);

            if (Chasing == false)
            {
                Chasing = true;
                StartCoroutine(ChaseAndAttack());
            }
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
            if (distance < attackDistance)
            {
                //attackIndicator.enabled = true;
                if (isAttacking == true)
                {
                    timeToAttack += Time.deltaTime;
                }

                if (timeToAttack >= defaultTimeToAttack / 1.5)
                {
                    weaponParent.Aim = false;
                    animations.aim = false;
                    Debug.Log("Stopping Aiming");
                }
                //attackIndicator.fillAmount = timeToAttack / defaultTimeToAttack;
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            //Getting the target
            aiData.currentTarget = aiData.targets[0];
        }

        //Debug.Log(movementInput);
        OnMovementInput?.Invoke(movementInput);
    }

    public void Attack()
    {
        Debug.Log("Swing");
        //abilityHolder.UseAbility = true; // <- Here for testing purposes.
        if (abilityHolder != null)
        {
            if (CurrentHealthPercentage <= abilityHolder.ability.usableAtHealthPercentage)
            {
                abilityHolder.UseAbility = true;
            }

        }
        weaponParent.Attack();
        if (hasAttackEffect) attackEffect.CancelAttack();
        if (hasAttackEffect) attackEffect.HeavyAttack();
    }
    public void UseAbility()
    {
        if (abilityHolder != null)
        {
            if (abilityHolder.ability.usableOutsideAttackRange == true)
            {
                if (CurrentHealthPercentage <= abilityHolder.ability.usableAtHealthPercentage)
                {
                    abilityHolder.UseAbility = true;
                }
            }
        }


        //if (abilityHolder.ability.usableOutsideAttackRange == true)
        //{
        //    abilityHolder.UseAbility = true;
        //}
    }

    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            //abilityHolder.CanUseAbility = false; // <- Here for testing purposes.
            if (abilityHolder != null)
            {
                abilityHolder.CanUseAbility = false;
            }
            isAttacking = false;
            if (hasAttackEffect) attackEffect.CancelAttack();
            indicatorAlive = false;
            movementInput = Vector2.zero;
            timeToAttack = 0;
            //attackIndicator.fillAmount = 0;
            Chasing = false;
            yield break;
        }
        else
        {
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
            if (distance < attackDistance)
            {
                movementInput = Vector2.zero;
                Debug.Log("Attacking");

                isAttacking = true;
                //Attacking 
                //abilityHolder.CanUseAbility = true; // <- Here for testing purposes.
                if (abilityHolder != null)
                {
                    abilityHolder.CanUseAbility = true;
                }
                //if (abilityHolder.UseAbility == false)
                //{
                //    movementInput = Vector2.zero;
                if (abilityHolder != null)
                {
                    if (abilityHolder.UseAbility == false)
                    {
                        movementInput = Vector2.zero;
                    }
                }
                //}
                OnAttackPressed?.Invoke();
                if (!indicatorAlive)
                {
                    indicatorAlive = true;

                    if (hasAttackEffect) attackEffect.AttackIndicator();
                    if (hasAttackEffect) attackEffect.SetIndicatorLifetime(defaultTimeToAttack);
                }



                if (timeToAttack >= defaultTimeToAttack) // Attack indicator stuff // Added timetoattack reset to chasing and idle states so that if player runs away it resets
                {
                    Attack(); // Attack method
                    indicatorAlive = false;
                    timeToAttack = 0;
                    //attackIndicator.fillAmount = 0;
                    isAttacking = false;
                }

                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                isAttacking = false;
                weaponParent.Aim = true;
                indicatorAlive = false;
                animations.aim = true;
                Debug.Log("Chasing");
                //Chasing
                //abilityHolder.CanUseAbility = true; // <- Here for testing purposes.
                if (abilityHolder != null)
                {
                    abilityHolder.CanUseAbility = true;
                }

                UseAbility();
                timeToAttack = 0;
                //attackIndicator.fillAmount = 0;
                if (hasAttackEffect) attackEffect.CancelAttack();
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
            }
        }
    }

    public override void Hit(float damage, HitType type, ICharacter source = null)
    {
        Health.TakeDamage(damage);
    }
}
