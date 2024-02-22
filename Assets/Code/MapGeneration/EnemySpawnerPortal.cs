using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EnemySpawnerPortal : MonoBehaviour
    {
        private Vector3 scale;
        private float scalingRate = 1f;
        private Vector3 zero = new Vector3(0f, 0f, 0f);
        private float timer = 3f;
        private float endTimer = 1f;
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private Sprite newSprite;
        private bool swap;
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            scale = transform.localScale;
            transform.localScale = zero;
        }

        // Update is called once per frame
        void Update()
        {
            if (timer > 0f)
            {
                transform.Rotate(0, 0, -200 * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, scale, scalingRate * Time.deltaTime);
                if (timer < 1f && !swap)
                {
                    swap = true;
                    spriteRenderer.sprite = newSprite;
                }
                timer -= Time.deltaTime;
            }
            else
            {
                transform.Rotate(0, 0, 800 * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, zero, scalingRate * 4 * Time.deltaTime);
                endTimer -= Time.deltaTime;
                if (endTimer <= 0)
                {
                    Destroy(gameObject);
                }
            }

        }
    }
}
