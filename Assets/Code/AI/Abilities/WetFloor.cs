using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/WetFloor")]
    public class WetFloor : Ability
    {
        public int maxHowManyPuddles = 0;
        public float SpawnRadius = 0;
        [SerializeField] private GameObject puddleObject;
        private GameObject spawnedPuddle;
        private GameObject[] puddleObjects;
        private Vector2 originPoint;
        private Vector2Int originPoint2int;
        private Vector3Int originPoint3int;
        public float spawnRadius;
        private Tilemap tileMap;
        private DestroyAbility destroy;
        public override void Activate(GameObject parent)
        {
            originPoint = parent.transform.position;
            tileMap = FindFirstObjectByType<Tilemap>();
            int randomPuddleCount = Random.Range(0, maxHowManyPuddles - 1);
            puddleObjects = new GameObject[randomPuddleCount];
            for(int i = 0; i < randomPuddleCount; i++)
            {
                originPoint = parent.transform.position + Random.insideUnitSphere * spawnRadius;
                originPoint2int = Vector2Int.RoundToInt(originPoint);
                originPoint3int = ((Vector3Int)originPoint2int);
                if (tileMap.HasTile(originPoint3int))
                {
                    puddleObjects[i] = Instantiate(puddleObject, originPoint, Quaternion.identity);
                    puddleObjects[i].GetComponent<Puddle>().damage = damage;
                    destroy = puddleObjects[i].AddComponent<DestroyAbility>();
                    destroy.activeTime = ActiveTime;
                }
            }
            DeactivateAbility();
        }

        private IEnumerator DeactivateAbility()
        {
            yield return new WaitForSeconds(coolDown + 1);
            foreach (GameObject puddle in puddleObjects)
            {
                Destroy(puddle);
            }
            
        }
        public override void Deactivate()
        {
            foreach(GameObject puddle in puddleObjects)
            {
                Destroy(puddle);
            }
            
        }

    }
}
