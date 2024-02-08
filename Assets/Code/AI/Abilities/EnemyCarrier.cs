using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pasta
{
    
    public class EnemyCarrier : MonoBehaviour
    {
        private Health health;
        [SerializeField] private GameObject Minion;
        private GameObject[] Minions;
        [SerializeField] private int howManytoSpawn = 1;
        public float SpawnRadius = 3f;
        private Vector2 originPoint;
        private Vector2Int originPoint2int;
        private Vector3Int originPoint3int;
        private Tilemap tileMap;
        private Level levelScript;
        private void Start()
        {
            health = GetComponent<Health>();
            tileMap = FindFirstObjectByType<Tilemap>();
            levelScript = FindFirstObjectByType<Level>();
        }

        

        public void SpawnMinions() // CALL THIS METHOD IN ENEMY AI AFTER CHECKING IF THAT ENEMY TYPE CAN SPAWN MINIONS
        {
            originPoint = transform.position;
            originPoint = transform.position + Random.insideUnitSphere * SpawnRadius;
            originPoint2int = Vector2Int.RoundToInt(originPoint);
            originPoint3int = ((Vector3Int)originPoint2int);
            if (tileMap.HasTile(originPoint3int))
            {
                Instantiate(Minion, originPoint, Quaternion.identity);
            }
            //for (int i = 0; i < howManytoSpawn; i++)
            //{
            //    //originPoint = transform.position;
            //    //originPoint = transform.position + Random.insideUnitSphere * SpawnRadius;
            //    //originPoint2int = Vector2Int.RoundToInt(originPoint);
            //    //originPoint3int = ((Vector3Int)originPoint2int);
            //    //if (tileMap.HasTile(originPoint3int))
            //    //{
            //    //    Instantiate(Minion, originPoint, Quaternion.identity);
            //    //}
            //}
        }
    }
}
