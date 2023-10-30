using Pasta;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimations : MonoBehaviour
{
    private AbilityHolder holder;
    public bool ActivateAbilityThroughAnim = false;
    [SerializeField] public Animator animator;
    [SerializeField] private GameObject EnemyBody;
    public bool aim = true;
    private EnemyAi enemyAi;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
        holder = GetComponent<AbilityHolder>();
        enemyAi = GetComponent<EnemyAi>();

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
            animator.SetBool("IsIdle", enemyAi.IsIdle);
            animator.SetFloat("DirX", enemyAi.movementInput.x);
            animator.SetFloat("DirY", enemyAi.movementInput.y);
        }
    }
    public void PlayAbilityAnim() // For playing enemy ability anims.
    {
        holder.AnimDone = false;
        animator.SetBool("Ability", true);
    }


    public void StopAbilityAnim()
    {
        animator.SetBool("Ability", false);
    }

    //public void PlayAnimation(Vector2 movementInput)
    //{
    //    animator.SetBool("Running", movementInput.magnitude > 0);

    //}
}
