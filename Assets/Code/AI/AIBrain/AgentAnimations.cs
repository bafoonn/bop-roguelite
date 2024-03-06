using Pasta;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AgentAnimations : MonoBehaviour
{
    private AbilityHolder holder;
    public bool ActivateAbilityThroughAnim = false;
    [SerializeField] public Animator animator;
    [SerializeField] private GameObject EnemyBody;
    public bool aim = true;
    public bool attacking = false;
    private EnemyAi enemyAi;
    private BossAI bossAi;
    private AgentMover agentMover;
    private float defaultSpeed;
    private Transform player;
    private FixedEnemyAI fixedAI;
    private RangedAI rangedAI;

    CleavingWeaponAnimations weaponAnimations;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
        holder = GetComponent<AbilityHolder>();
        enemyAi = GetComponent<EnemyAi>();
        agentMover = GetComponent<AgentMover>();
        defaultSpeed = agentMover.maxSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        fixedAI = GetComponent<FixedEnemyAI>();
        rangedAI = GetComponent<RangedAI>();
        bossAi = GetComponent<BossAI>();
    }

    public void RotateToPointer(Vector2 lookDirection)
    {
        if (aim)
        {
            weaponAnimations = GetComponentInChildren<CleavingWeaponAnimations>();
            if(weaponAnimations != null )
            {
                weaponAnimations.SetAim(lookDirection);
            }
           
            float x = Mathf.Abs(EnemyBody.transform.localScale.x);
            if (lookDirection.x < 0)
            {
                x *= -1;
            }
            Vector3 newScale = EnemyBody.transform.localScale;
            newScale.x = x;
            EnemyBody.transform.localScale = newScale;
        }

    }

    private void Update()
    {
        if (animator != null)
        {
            if (enemyAi != null)
            {
                animator.SetBool("isAttacking", enemyAi.isAttacking);
                animator.SetBool("IsIdle", enemyAi.IsIdle);
                animator.SetBool("Death", enemyAi.Death);
                animator.SetFloat("DirX", enemyAi.movementInput.x);
                animator.SetFloat("DirY", enemyAi.movementInput.y);
            }
            if(fixedAI != null)
            {
                
                animator.SetBool("isAttacking", fixedAI.isAttacking);
                animator.SetBool("IsIdle", fixedAI.IsIdle);
                animator.SetBool("Death", fixedAI.Death);
                animator.SetFloat("DirX", fixedAI.movementInput.x);
                animator.SetFloat("DirY", fixedAI.movementInput.y);
            }
            if(rangedAI != null)
            {
                animator.SetBool("isAttacking", rangedAI.isAttacking);
                animator.SetBool("IsIdle", rangedAI.IsIdle);
                animator.SetBool("Death", rangedAI.Death);
                animator.SetFloat("DirX", rangedAI.movementInput.x);
                animator.SetFloat("DirY", rangedAI.movementInput.y);
            }
            if(bossAi != null)
			{
                animator.SetBool("isAttacking", bossAi.isAttacking);
                animator.SetBool("IsIdle", bossAi.IsIdle);
                animator.SetBool("Death", bossAi.Death);
                animator.SetFloat("DirX", bossAi.movementInput.x);
                animator.SetFloat("DirY", bossAi.movementInput.y);
            }


        }
    }


    public void PlayAbilityAnim() // For playing enemy ability anims.
    {
        agentMover.Speed = 0f;
        holder.AnimDone = false;
        animator.SetBool("Ability", true);
    }


    public void StopAbilityAnim()
    {
        agentMover.Speed = agentMover.BaseSpeed;
        animator.SetBool("Ability", false);
    }
}
