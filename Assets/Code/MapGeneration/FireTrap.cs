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
        private float timerSet = 5f;
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
                Activate();
            }
            timer = timerSet;
        }

        private void Activate()
        {
            animator.SetTrigger("Activate");
            animator.SetTrigger("On");
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
        }
    }
}
