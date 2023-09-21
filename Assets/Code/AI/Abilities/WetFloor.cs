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
        private GameObject[] puddleObjects;
        private Vector2 originPoint;
        private Vector2Int originPoint2int;
        private Vector3Int originPoint3int;
        public float spawnRadius;
        private Tilemap tileMap;
        public override void Activate(GameObject parent)
        {
            originPoint2int = Vector2Int.RoundToInt(originPoint);
            originPoint3int = ((Vector3Int)originPoint2int);
            tileMap = FindFirstObjectByType<Tilemap>();
            float randomPuddleCount = Random.Range(0, maxHowManyPuddles);
            for(int i = 0; i < randomPuddleCount; i++)
            {
                originPoint = Random.insideUnitSphere * spawnRadius;
                if (tileMap.HasTile(originPoint3int))
                {
                    Instantiate(puddleObject, originPoint, Quaternion.identity);
                    puddleObjects[i] = puddleObject;
                }
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
