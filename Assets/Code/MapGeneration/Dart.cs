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

        private int dartdamage;
        private bool isActive = false;

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isActive)
            {
                transform.position += transform.right * Time.deltaTime * speed;
            }
        }

        public void StartMoving(int damage)
        {
            dartdamage = damage;
            isActive = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (layers.Includes(col.gameObject.layer))
            {
                if (col.TryGetComponent<IHittable>(out IHittable hittable))
                {
                    hittable.Hit(dartdamage);
                }
                Destroy(gameObject);
            }
        }
    }
}
