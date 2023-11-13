using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        public float damage = 2f;
        public LayerMask WhatLayerDestroysThis;

        // Start is called before the first frame update
        void Start()
        {
            Invoke("OnDestroy", 4f);
        }

        // Update is called once per frame
        void Update()
        {

            transform.Translate(Vector3.right * speed * Time.deltaTime);

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (collision.TryGetComponent<IHittable>(out var hittable))
                {
                    hittable.Hit(damage);
                }
            }
            else if (WhatLayerDestroysThis.Includes(collision.gameObject.layer) && collision.gameObject.tag != "Enemy")
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            Destroy(gameObject);
        }
    }
}
