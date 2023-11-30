using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/BulletHell")]
    public class BulletHell : Ability
    {
        [SerializeField] private int MaxHowManySpawners = 4;
        public float SpawnRadius = 10f;
        [SerializeField] private GameObject spawnerBullets;
        private GameObject[] spawners;
        private Tilemap tileMap;
        private DestroyAbility destroy;
        private Vector2 originPoint;
        private Vector2Int originPoint2int;
        private Vector3Int originPoint3int;
        public override void Activate(GameObject parent)
        {
            tileMap = FindFirstObjectByType<Tilemap>();
            for (int i = 0; i < MaxHowManySpawners; i++)
            {
                originPoint = parent.transform.position + Random.insideUnitSphere * SpawnRadius;
                originPoint2int = Vector2Int.RoundToInt(originPoint);
                originPoint3int = ((Vector3Int)originPoint2int);
                if (tileMap.HasTile(originPoint3int))
                {
                    spawners[i] = Instantiate(spawnerBullets, originPoint, Quaternion.identity);
                    spawners[i].GetComponent<DealDamageOnTrigger>().damage = damage;
                    destroy = spawners[i].AddComponent<DestroyAbility>();
                    destroy.activeTime = ActiveTime;
                }
            }
            DeactivateAbility();
        }

        private IEnumerator DeactivateAbility()
        {
            yield return new WaitForSeconds(coolDown + 1);
            foreach (GameObject puddle in spawners)
            {
                Destroy(puddle);
            }

        }
        public override void Deactivate()
        {
            foreach (GameObject sapwner in spawners)
            {
                Destroy(sapwner);
            }
        }
    }
}
