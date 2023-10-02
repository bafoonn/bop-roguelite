using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] enemies;
        [SerializeField]
        private int totalEnemiesToSpawn = 1;
        [SerializeField]
        private float startDelay;
        [SerializeField]
        private float spawnDelay;
        private int enemiesToSpawn;
        private Level level;
        private bool isSpawning;
        private bool enemiesSpawned = false;
        // Start is called before the first frame update
        void Start()
        {
            enemiesToSpawn = totalEnemiesToSpawn;
            level = GetComponentInParent<Level>();
            level.AddToEnemyCount(totalEnemiesToSpawn);
            isSpawning = false;
        }
        void Update()
        {
            if (isSpawning == false && enemiesSpawned == false)
            {
                StartCoroutine(SpawnRoutine());
            }
        }

        private IEnumerator SpawnRoutine()
        {
            isSpawning = true;
            if (enemiesToSpawn == totalEnemiesToSpawn && startDelay != 0)
            {
                yield return new WaitForSeconds(startDelay);
            }
            else if (enemiesToSpawn != totalEnemiesToSpawn && spawnDelay != 0)
            {
                yield return new WaitForSeconds(spawnDelay);
            }
            int random = Random.Range(0, enemies.Length);
            Instantiate(enemies[random], transform.position, Quaternion.identity);
            enemiesToSpawn--;
            if (enemiesToSpawn == 0)
            {
                enemiesSpawned = true;
            }
            isSpawning = false;
        }
    }
}
