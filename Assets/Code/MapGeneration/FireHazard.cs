using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class FireHazard : MonoBehaviour
    {
        [SerializeField]
        private int damage;
        private bool isActive;
        [SerializeField]
        private float timerSet = 2f;
        private float timer;
        private SpriteRenderer spriteRenderer;
        private Color baseColor;
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            baseColor = spriteRenderer.color;
            spriteRenderer.material.SetColor("_Color", Color.black);
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
        private void OnTriggerStay2D(Collider2D col)
        {
            if (isActive && col.TryGetComponent(out Health health))
            {
                if (!health.CheckIfTakenTrapDamage())
                {
                    health.TakeDamage(damage);
                    health.TrapDamageTaken();
                }
            }
        }

        private void SwapModes()
        {
            if (isActive)
            {
                isActive = false;
                spriteRenderer.material.SetColor("_Color", Color.black);
            }
            else
            {
                isActive = true;
                spriteRenderer.material.SetColor("_Color", baseColor);
            }
            timer = timerSet;
        }
    }
}
