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
        private int totalEnemiesToSpawnPerWave = 1;
        [SerializeField]
        private int waves = 1;
        [SerializeField]
        private float startDelay;
        [SerializeField]
        private float spawnDelay;
        [SerializeField]
        private bool randomSpawn = true;
        private int enemiesToSpawn;
        private Level level;
        private bool isSpawning;
        private bool allEnemiesSpawned = false;
        private bool spawnWave = true;
        private bool firstSpawn = false;
        private int enemyIndex = 0;
        // Start is called before the first frame update
        void Start()
        {
            enemiesToSpawn = totalEnemiesToSpawnPerWave;
            level = GetComponentInParent<Level>();
            level.AddToEnemyCount(totalEnemiesToSpawnPerWave);
            isSpawning = false;
        }
        void Update()
        {
            if (isSpawning == false && allEnemiesSpawned == false && spawnWave == true)
            {
                StartCoroutine(SpawnRoutine());
            }
        }

        private IEnumerator SpawnRoutine()
        {
            isSpawning = true;
            if (enemiesToSpawn == totalEnemiesToSpawnPerWave && startDelay != 0)
            {
                yield return new WaitForSeconds(startDelay);
            }

            else if (enemiesToSpawn != totalEnemiesToSpawnPerWave && spawnDelay != 0 && firstSpawn)
            {
                yield return new WaitForSeconds(spawnDelay);
            }

            if (randomSpawn)
            {
                int random = Random.Range(0, enemies.Length);
                GameObject enemy = Instantiate(enemies[random], transform.position, Quaternion.identity);
                enemy.transform.SetParent(level.gameObject.transform);
                enemiesToSpawn--;
            }
            else
            {
                GameObject enemy = Instantiate(enemies[enemyIndex], transform.position, Quaternion.identity);
                enemy.transform.SetParent(level.gameObject.transform);
                enemiesToSpawn--;
                enemyIndex++;
            }
            if (!firstSpawn)
            {
                firstSpawn = true;
            }
            if (enemiesToSpawn == 0)
            {
                waves--;
                firstSpawn = false;
                spawnWave = false;
                if (waves == 0)
                {
                    allEnemiesSpawned = true;
                }
            }

            isSpawning = false;
        }

        public int wavesCheck()
        {
            return waves;
        }
        public void startNewWave()
        {
            if (waves != 0)
            {
                spawnWave = true;
            }
        }
    }
}
