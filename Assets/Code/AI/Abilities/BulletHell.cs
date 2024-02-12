using System.Collections;
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
        public GameObject spawnedSpawner;

        private Tilemap tileMap;
        private DestroyAbility destroy;
        private Vector2 originPoint;
        private Vector2Int originPoint2int;
        private Vector3Int originPoint3int;
        public override void Activate(GameObject parent)
        {
            tileMap = FindFirstObjectByType<Tilemap>();

            originPoint = parent.transform.position + Random.insideUnitSphere * SpawnRadius;
            originPoint2int = Vector2Int.RoundToInt(originPoint);
            originPoint3int = ((Vector3Int)originPoint2int);
            if (tileMap.HasTile(originPoint3int))
            {
                spawnedSpawner = Instantiate(spawnerBullets, originPoint, Quaternion.identity);
                destroy = spawnedSpawner.AddComponent<DestroyAbility>();
                destroy.activeTime = ActiveTime;
            }

            DeactivateAbility();
        }

        private IEnumerator DeactivateAbility()
        {
            yield return new WaitForSeconds(coolDown + 1);

            Destroy(spawnedSpawner);


        }
        public override void Deactivate()
        {
            Destroy(spawnedSpawner);
        }
    }
}
