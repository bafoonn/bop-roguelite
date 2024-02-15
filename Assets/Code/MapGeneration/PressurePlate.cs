using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PressurePlate : MonoBehaviour
    {
        private DartTrap dartTrap;
        private bool isActive = true;
        private BoxCollider2D boxCol;
        private SpriteRenderer spriteRenderer;
        private Color baseColor;
        private Animator animator;
        private bool stay;
        private bool disableOffAnimation;
        private void Start()
        {
            dartTrap = GetComponentInParent<DartTrap>();
            boxCol = gameObject.GetComponent<BoxCollider2D>();
            animator = GetComponent<Animator>();
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out IHittable hittable))
            {
                stay = true;
                animator.SetTrigger("On");
                animator.SetTrigger("Stay");
                animator.ResetTrigger("Off");
                animator.ResetTrigger("Default");
                dartTrap.ActivateTrap();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IHittable hittable))
            {
                if (!disableOffAnimation)
                {
                    animator.SetTrigger("Off");
                    animator.SetTrigger("Default");
                    animator.ResetTrigger("On");
                }
                else
                {
                    disableOffAnimation = false;
                }
                stay = false;
            }
        }

        public void ResetPlate()
        {
            if (stay)
            {
                disableOffAnimation = true;
                boxCol.enabled = false;
                boxCol.enabled = true;
            }
            else
            {
                disableOffAnimation = false;
            }
        }
    }
}
