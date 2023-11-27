using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class SpikeHazard : MonoBehaviour
    {
        [SerializeField]
        private int damage = 20;
        private bool isActive;
        private bool spikesTriggered;
        private SpriteRenderer spriteRenderer;
        private Color baseColor;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            baseColor = spriteRenderer.color;
            spriteRenderer.material.SetColor("_Color", Color.black);
        }
        private void OnTriggerStay2D(Collider2D col)
        {
            if (!spikesTriggered && col.GetComponent<Health>())
            {
                StartCoroutine(ActivateSpikes());
            }

            if (isActive && col.TryGetComponent(out Health health))
            {
                if (!health.CheckIfTakenTrapDamage())
                {
                    health.TakeDamage(damage);
                    health.TrapDamageTaken();
                }
            }
        }


        IEnumerator ActivateSpikes()
        {
            spikesTriggered = true;
            yield return new WaitForSeconds(1f);
            spriteRenderer.material.SetColor("_Color", Color.blue);
            isActive = true;
            yield return new WaitForSeconds(1f);
            spriteRenderer.material.SetColor("_Color", Color.black);
            spikesTriggered = false;
            isActive = false;
        }
    }
}
