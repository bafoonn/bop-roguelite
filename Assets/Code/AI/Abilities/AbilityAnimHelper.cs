using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    
    public class AbilityAnimHelper : MonoBehaviour
    {
        private AbilityHolder holder;
        public bool ActivateAbilityThroughAnim = false;
        [SerializeField] public Animator animator;
        [SerializeField] private Animation animationClip;

        private void Start()
        {
            holder = GetComponent<AbilityHolder>();
        }

        public void PlayAnim()
        {
            holder.AnimDone = false;
            animator.SetBool("Ability", true);
        }


        public void StopAnim()
        {
            Debug.Log("Stopping Anim");
            animator.SetBool("Ability", false);
        }


    }
}
