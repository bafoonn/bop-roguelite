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
    private AgentMover agentMover;
    private float defaultSpeed;
    private Transform player;
    private void Awake()
    {
        //animator = GetComponent<Animator>();
        holder = GetComponent<AbilityHolder>();
        enemyAi = GetComponent<EnemyAi>();
        agentMover = GetComponent<AgentMover>();
        defaultSpeed = agentMover.maxSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void RotateToPointer(Vector2 lookDirection)
    {
        if (aim)
        {
            Vector3 scale = transform.localScale;
            if (lookDirection.x > 0)
            {
                scale.x = 1;
            }
            else if (lookDirection.x < 0)
            {
                scale.x = -1;
            }
            EnemyBody.transform.localScale = scale;
        }

    }

    private void Update()
    {
        if (animator != null)
        {
            animator.SetBool("isAttacking", enemyAi.isAttacking);
            animator.SetBool("IsIdle", enemyAi.IsIdle);
            animator.SetBool("Death", enemyAi.Death);
            animator.SetFloat("DirX", enemyAi.movementInput.x);
            animator.SetFloat("DirY", enemyAi.movementInput.y);
           
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
