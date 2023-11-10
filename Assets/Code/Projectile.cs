using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Pasta
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        public float damage = 2f;
        public LayerMask WhatLayerDestroysThis;
        int layer;

        // Start is called before the first frame update
        void Start()
        {
            Invoke("OnDestroy", 4f);
            layer = WhatLayerDestroysThis;
        }

        // Update is called once per frame
        void Update()
        {
            
            transform.Translate(Vector3.right * speed * Time.deltaTime);

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                if (collision.TryGetComponent<IHittable>(out var hittable))
                {
                    hittable.Hit(damage);
                }
            }
            else if(collision.gameObject.layer == WhatLayerDestroysThis)
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
