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
            spriteRenderer.material.SetColor("_Color", Color.black);
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
            if (isActive && col.TryGetComponent(out IHittable hittable))
            {
                hittable.Hit(damage);
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
                boxcol.enabled = false;
                boxcol.enabled = true;
                isActive = true;
                spriteRenderer.material.SetColor("_Color", baseColor);
            }
            timer = timerSet;
        }
    }
}
