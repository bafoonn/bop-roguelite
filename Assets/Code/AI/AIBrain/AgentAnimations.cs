using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimations : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private GameObject EnemyBody;
    public bool aim = true;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
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

    //public void PlayAnimation(Vector2 movementInput)
    //{
    //    animator.SetBool("Running", movementInput.magnitude > 0);

    //}
}
