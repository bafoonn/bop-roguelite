using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class FrostSpikeTrap : Trap
    {
        [SerializeField]
        private int damage = 5;
        private bool isActive;
        private bool spikesTriggered;
        private BoxCollider2D boxcol;
        private Animator animator;

        private void Start()
        {
            boxcol = gameObject.GetComponent<BoxCollider2D>();
            animator = GetComponent<Animator>();
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!Disabled)
            {
                if (!spikesTriggered && col.TryGetComponent<IHittable>(out _))
                {
                    StartCoroutine(ActivateSpikes());
                }

                if (isActive && col.TryGetComponent(out IHittable hittable) && col.TryGetComponent(out Health health) && col.TryGetComponent(out ICharacter character))
                {
                    if (!health.DealTrapDamage())
                    {
                        hittable.Hit(damage);
                        character.Status.ApplyStatus(new SlowStatus(0.4f), 2f);
                    }
                }
            }
        }


        IEnumerator ActivateSpikes()
        {
            spikesTriggered = true;
            animator.SetTrigger("Activate");
            yield return new WaitForSeconds(1f);
            animator.SetTrigger("Spring");
            yield return new WaitForSeconds(0.3f);
            isActive = true;
            boxcol.enabled = false;
            boxcol.enabled = true;
            yield return new WaitForSeconds(1f);
            animator.SetTrigger("Deactivate");
            yield return new WaitForSeconds(0.2f);
            animator.SetTrigger("Default");
            spikesTriggered = false;
            isActive = false;
            boxcol.enabled = false;
            boxcol.enabled = true;
        }
    }
}
