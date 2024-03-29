using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class FireTrap : Trap
    {
        [SerializeField]
        private int damage;
        private bool isActive;
        [SerializeField]
        private float timerSet = 2f;
        private float timer;
        private Animator animator;
        private BoxCollider2D boxcol;
        private void Start()
        {
            animator = GetComponent<Animator>();
            boxcol = gameObject.GetComponent<BoxCollider2D>();
            timer = timerSet;
        }
        private void Update()
        {
            if (!Disabled)
            {
                if (timer >= 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    SwapModes();
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (isActive && col.TryGetComponent(out ICharacter character) && col.TryGetComponent(out Health health))
            {
                if (col.TryGetComponent(out Player player))
                {
                    if (player.CheckIfIFrames())
                    {
                        return;
                    }
                }
                if (!health.DealTrapDamage())
                {
                    character.Status.ApplyStatus(new BurnStatus(damage), 1f);
                }
            }
        }

        private void SwapModes()
        {
            if (isActive)
            {
                StartCoroutine(Deactivate());
            }
            else
            {
                StartCoroutine(Activate());
            }
            timer = timerSet;
        }

        IEnumerator Activate()
        {
            animator.SetTrigger("Activate");
            animator.SetTrigger("On");
            yield return new WaitForSeconds(0.3f);
            boxcol.enabled = false;
            boxcol.enabled = true;
            isActive = true;
        }
        
        IEnumerator Deactivate()
        {
            isActive = false;
            animator.SetTrigger("Deactivate");
            yield return new WaitForSeconds(0.5f);
            animator.SetTrigger("Default");
        }

        public override void Disable()
        {
            if (isActive)
            {
                isActive = false;
            }

            Disabled = true;
        }
    }
}
