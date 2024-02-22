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
        private int[] totalEnemiesToSpawnPerWave;
        private int waves = 1;
        private int currentWave = 0;
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
        [SerializeField]
        private EnemySpawnerPortal portal;
        private bool preSpawn;
        // Start is called before the first frame update
        void Start()
        {
            waves = totalEnemiesToSpawnPerWave.Length;
            enemiesToSpawn = totalEnemiesToSpawnPerWave[currentWave];
            level = GetComponentInParent<Level>();
            isSpawning = false;
        }
        void Update()
        {
            if (isSpawning == false && allEnemiesSpawned == false && spawnWave == true && !preSpawn)
            {
                StartCoroutine(PreSpawnRoutine());
            }
            else if (isSpawning == false && allEnemiesSpawned == false && spawnWave == true && preSpawn)
            {
                StartCoroutine(SpawnRoutine());
            }
        }

        private IEnumerator PreSpawnRoutine()
        {
            isSpawning = true;
            Debug.Log(currentWave);
            if (totalEnemiesToSpawnPerWave[currentWave] != 0)
            {
                Instantiate(portal, transform.position, Quaternion.identity, transform);
            }
            yield return new WaitForSeconds(2.2f);
            preSpawn = true;
            isSpawning = false;
        }

        private IEnumerator SpawnRoutine()
        {
            isSpawning = true;

            //Delay between each spawn
            if (enemiesToSpawn != totalEnemiesToSpawnPerWave[currentWave] && spawnDelay != 0 && firstSpawn)
            {
                yield return new WaitForSeconds(spawnDelay);
            }

            //Spawn a random enemy from the list if randomSpawn is checked
            if (randomSpawn)
            {
                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    int random = Random.Range(0, enemies.Length);
                    GameObject enemy = Instantiate(enemies[random], transform.position, Quaternion.identity);
                    enemy.transform.SetParent(level.gameObject.transform);
                    enemiesToSpawn--;
                }
            }
            //Spawn enemies in order from the list
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
            //Reset the routine for new wave or end the routine if no waves left
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
            preSpawn = false;
        }

        public int wavesCheck()
        {
            return waves;
        }
        public void startNewWave()
        {
            if (waves != 0)
            {
                currentWave++;
                enemiesToSpawn = totalEnemiesToSpawnPerWave[currentWave];
                spawnWave = true;
            }
        }
    }
}
