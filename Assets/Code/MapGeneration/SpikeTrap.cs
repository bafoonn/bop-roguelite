using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class SpikeTrap : MonoBehaviour
    {
        [SerializeField]
        private int damage = 5;
        private bool isActive;
        private bool spikesTriggered;
        private SpriteRenderer spriteRenderer;
        private Color baseColor;
        private BoxCollider2D boxcol;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            baseColor = spriteRenderer.color;
            spriteRenderer.material.SetColor("_Color", Color.grey);
            boxcol = gameObject.GetComponent<BoxCollider2D>();
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!spikesTriggered && col.TryGetComponent<IHittable>(out _))
            {
                StartCoroutine(ActivateSpikes());
            }

            if (isActive && col.TryGetComponent(out IHittable hittable) && col.TryGetComponent(out Health health))
            {   
                if (!health.DealTrapDamage())
                { 
                    hittable.Hit(damage); 
                }
            }
        }


        IEnumerator ActivateSpikes()
        {
            spikesTriggered = true;
            yield return new WaitForSeconds(1f);
            spriteRenderer.material.SetColor("_Color", Color.blue);
            isActive = true;
            boxcol.enabled = false;
            boxcol.enabled = true;
            yield return new WaitForSeconds(1f);
            spriteRenderer.material.SetColor("_Color", Color.grey);
            spikesTriggered = false;
            isActive = false;
        }
    }
}
