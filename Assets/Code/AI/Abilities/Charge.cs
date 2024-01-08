using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/Teleport")]
    public class Charge : Ability
    {
        private Rigidbody2D rigidbody;
        private AIData aiData;
        public float speed = 0f;
        public float distancetoUse;
        private Vector2 TargetPos;
        private BoxCollider2D BoxCollider2d;
        private Transform player;
        private BoxCollider2D bC2D;
        private GameObject tilemapholder;
        private Tilemap tilemap;
        private Vector2Int v2i;
        private Vector3Int v3i;
        public override void Activate(GameObject parent)
        {
            tilemap = FindFirstObjectByType<Tilemap>();
            AIData aiData = parent.GetComponent<AIData>();
            //BossAI bossAi = parent.GetComponent<BossAI>();
            //GameObject player = GameObject.FindGameObjectWithTag("Player");

            rigidbody = parent.GetComponent<Rigidbody2D>();
            BoxCollider2d = parent.GetComponent<BoxCollider2D>();
            if (aiData.currentTarget != null) {
                TargetPos = aiData.currentTarget.position + Random.insideUnitSphere;
                v2i = Vector2Int.RoundToInt(TargetPos);
                v3i = ((Vector3Int)v2i);
                if (tilemap.HasTile(v3i))
                {
                    Debug.Log("Hastile");
                    rigidbody.position = TargetPos;
                }
                else
                {
                    Debug.Log("notile");
                }
			}
			else
			{
                TargetPos = player.position;
                v2i = Vector2Int.RoundToInt(TargetPos);
                v3i = ((Vector3Int)v2i);
                if (tilemap.HasTile(v3i))
                {
                    Debug.Log("Hastile");
                    rigidbody.position = TargetPos;
                }
                else
                {
                    Debug.Log("notile");
                }
            }
            if(tilemap == null)
            {
                Debug.Log("Tilemap not found");
            }
            //rigidbody.MovePosition(TargetPos * speed);
            
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
            if (bC2D == null) return;
            if (rigidbody == null) return;
            if (BoxCollider2d == null) return;

            Destroy(bC2D);

            rigidbody.velocity = Vector2.zero;
            BoxCollider2d.isTrigger = false;

        }
    }
}
