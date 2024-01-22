using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class FireTrap : MonoBehaviour
    {
        [SerializeField]
        private int damage;
        private bool isActive;
        [SerializeField]
        private float timerSet = 2f;
        private float timer;
        private SpriteRenderer spriteRenderer;
        private Color baseColor;
        private BoxCollider2D boxcol;
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            baseColor = spriteRenderer.color;
            spriteRenderer.material.SetColor("_Color", Color.grey);
            boxcol = gameObject.GetComponent<BoxCollider2D>();
            timer = timerSet;
        }
        private void Update()
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
                isActive = false;
                spriteRenderer.material.SetColor("_Color", Color.grey);
            }
            else
            {
                boxcol.enabled = false;
                boxcol.enabled = true;
                isActive = true;
                spriteRenderer.material.SetColor("_Color", baseColor);
            }
            timer = timerSet;
        }
    }
}
