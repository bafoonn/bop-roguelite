using Pasta;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimations : MonoBehaviour
{
    private AbilityHolder holder;
    public bool ActivateAbilityThroughAnim = false;
    [SerializeField] public Animator animator;
    [SerializeField] private Animation animationClip;
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
            animator.SetFloat("DirX", enemyAi.movementInput.x);
            animator.SetFloat("DirY", enemyAi.movementInput.y);
        }
    }
    public void PlayAbilityAnim()
    {
        holder.AnimDone = false;
        animator.SetBool("Ability", true);
    }


    public void StopAbilityAnim()
    {
        Debug.Log("Stopping Anim");
        animator.SetBool("Ability", false);
    }

    //public void PlayAnimation(Vector2 movementInput)
    //{
    //    animator.SetBool("Running", movementInput.magnitude > 0);

    //}
}
