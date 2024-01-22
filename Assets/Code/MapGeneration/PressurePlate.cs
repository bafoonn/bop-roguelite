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
            if (isActive && col.TryGetComponent<IHittable>(out _))
            {
                dartTrap.ActivaeTrap();
            }
        }

        public IEnumerator OffTimer()
        {
            isActive = false;
            spriteRenderer.material.SetColor("_Color", Color.grey);
            yield return new WaitForSeconds(3f);
            spriteRenderer.material.SetColor("_Color", baseColor);
            boxCol.enabled = false;
            boxCol.enabled = true;
            isActive = true;

        }
    }
}
