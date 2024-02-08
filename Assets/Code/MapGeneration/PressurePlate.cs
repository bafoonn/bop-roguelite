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
        private void Start()
        {
            dartTrap = GetComponentInParent<DartTrap>();
            boxCol = gameObject.GetComponent<BoxCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            baseColor = spriteRenderer.color;
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<IHittable>(out _))
            {
                spriteRenderer.material.SetColor("_Color", Color.grey);
                dartTrap.ActivateTrap();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IHittable>(out _))
            {
                spriteRenderer.material.SetColor("_Color", baseColor);
            }
        }

        public void ResetPlate()
        {
            boxCol.enabled = false;
            boxCol.enabled = true;
        }
    }
}
