using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/Teleport")]
    public class Charge : Ability
    {
        private Rigidbody2D rigidbody;
        private AIData aiData;
        public float speed = 0f;
        public float distancetoUse;

        private BoxCollider2D BoxCollider2d;

        private BoxCollider2D bC2D;
        public override void Activate(GameObject parent)
        {
            AIData aiData = parent.GetComponent<AIData>();
            //BossAI bossAi = parent.GetComponent<BossAI>();
            //GameObject player = GameObject.FindGameObjectWithTag("Player");

            rigidbody = parent.GetComponent<Rigidbody2D>();
            BoxCollider2d = parent.GetComponent<BoxCollider2D>();
            Vector2 TargetPos = aiData.currentTarget.position + Random.insideUnitSphere;
            //rigidbody.MovePosition(TargetPos * speed);
            rigidbody.position = TargetPos;
            bC2D = parent.AddComponent<BoxCollider2D>();
            BoxCollider2d.isTrigger = true;
            bC2D.isTrigger = true;
            bC2D.size = new Vector2(1, 1);
            bC2D.edgeRadius = 0.9f;
            LayerMask mask = LayerMask.GetMask("Player");
            Collider2D[] colliders = Physics2D.OverlapBoxAll(parent.transform.position, bC2D.size, 0, mask);
            Debug.Log(colliders.Length);
            foreach (Collider2D collider2D in colliders)
            {
                if (collider2D.TryGetComponent(out IHittable hittable))
                {
                    //bossAi.movementInput = Vector2.zero;

                    hittable.Hit(damage);// DO DAMAGE!
                }
            }


        }

        public override void Deactivate()
        {
            Destroy(bC2D);
            //rigidbody.velocity = Vector2.zero;
            BoxCollider2d.isTrigger = false;

        }
    }
}
