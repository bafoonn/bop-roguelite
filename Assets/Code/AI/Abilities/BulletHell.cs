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
        public LayerMask floorLayer;

		
		public override void Activate(GameObject parent)
        {

#pragma warning disable CS0618 // FindObjectsOfType is obsolete
            Tilemap[] allTilemaps = FindObjectsOfType<Tilemap>();
#pragma warning restore CS0618 // FindObjectsOfType is obsolete


            foreach (Tilemap tilemap in allTilemaps)
            {
                // Check if the current tilemap has the desired layer (e.g., "Floor")
                if (tilemap.gameObject.layer == LayerMask.NameToLayer("Floor"))
                {
                    tileMap = tilemap;
                    break; // Stop iterating once you find the correct tilemap
                }
            }
            Debug.Log(tileMap + " :tilemap");
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
