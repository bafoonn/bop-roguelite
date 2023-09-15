using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu]
    public class Charge : Ability
    {
        private Rigidbody2D rigidbody;
        private AIData aiData;
        public float speed = 10f;

        private BoxCollider2D bC2D;
        public override void Activate(GameObject parent)
        {
            AIData aiData = parent.GetComponent<AIData>();
            rigidbody = parent.GetComponent<Rigidbody2D>();
            rigidbody.velocity = aiData.currentTarget.transform.position * speed;
            bC2D = parent.AddComponent<BoxCollider2D>();
            bC2D.size = new Vector2(1, 3);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(parent.transform.position, bC2D.size, 0, 6);
            foreach (Collider2D collider2D in colliders)
            {
                if (collider2D.TryGetComponent(out IHittable hittable))
                {
                    hittable.Hit(damage);// DO DAMAGE!
                }
            }


        }

        public override void Deactivate(GameObject parent)
        {
            bC2D.enabled = false;
            rigidbody.velocity = Vector2.zero;
        }
    }
}
