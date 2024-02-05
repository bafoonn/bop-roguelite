using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Dart : MonoBehaviour
    {
        [SerializeField]
        private float speed = 1f;
        [SerializeField]
        private LayerMask layers;

        private float dartdamage;
        private bool isActive = false;
        private bool isColActive;

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isActive)
            {
                transform.position += transform.right * Time.deltaTime * speed;
            }
        }

        public void StartMoving(float damage)
        {
            dartdamage = damage;
            isActive = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (layers.Includes(col.gameObject.layer) && isColActive)
            {
                if (col.TryGetComponent<IHittable>(out IHittable hittable))
                {
                    if (col.TryGetComponent<Player>(out Player player))
                    {
                        if(player.CheckIfIFrames())
                        {
                            return;
                        }
                    }
                    hittable.Hit(dartdamage);
                }
                Destroy(gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            isColActive = true;
        }
    }
}
