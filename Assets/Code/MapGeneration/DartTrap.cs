using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class DartTrap : MonoBehaviour
    {
        [SerializeField]
        private Transform[] dartShooters;
        [SerializeField]
        private Dart dart;
        private Dart activeDart;
        [SerializeField]
        private int damage = 30;
        private bool isActivated = false;
        private SpriteRenderer spriteRenderer;
        private Color baseColor;
        private BoxCollider2D boxcol;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            baseColor = spriteRenderer.color;
            boxcol = gameObject.GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!isActivated && col.TryGetComponent<IHittable>(out _))
            {
                spriteRenderer.material.SetColor("_Color", Color.black);
                for (int i = 0; i < dartShooters.Length; i++)
                {
                    activeDart = Instantiate(dart, dartShooters[i].transform.position, Quaternion.identity);
                    activeDart.transform.up = transform.position - activeDart.transform.position;
                    SpriteRenderer sprite = activeDart.GetComponent<SpriteRenderer>();
                    sprite.transform.Rotate(0, 0, 90);
                    activeDart.StartMoving(damage);
                }
                StartCoroutine(OffTimer());
            }
        }

        IEnumerator OffTimer()
        {
            isActivated = true;
            yield return new WaitForSeconds(2f);
            isActivated = false;
            spriteRenderer.material.SetColor("_Color", baseColor);
            boxcol.enabled = false;
            boxcol.enabled = true;
        }
    }
}
