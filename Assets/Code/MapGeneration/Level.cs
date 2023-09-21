using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Level : MonoBehaviour
    {
        private EndPoints endPoints;
        private RoomRewardSpawner rewardSpawner;
        private GameObject player;
        private int enemiesLeft;
        private int rewardIndex;

        [SerializeField]
        private Transform spawnPoint;

        // Start is called before the first frame update
        void Start()
        {
            endPoints = GetComponentInChildren<EndPoints>();
            endPoints.gameObject.SetActive(false);
            rewardSpawner = GetComponentInChildren<RoomRewardSpawner>();
            rewardSpawner.gameObject.SetActive(false);
            player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = spawnPoint.transform.position;
        }

        public void PassRewardIndex(int index)
        {
            rewardIndex = index;
        }
        // Called whenever an EnemySpawner spawns an enemy
        public void AddToEnemyCount()
        {
            enemiesLeft++;
            Debug.Log(enemiesLeft);
        }
        // Called whenever an enemy dies, at 0 enemies left activates endpoints that start the next level generation
        public void EnemyKilled()
        {
            enemiesLeft--;
            if (enemiesLeft == 0)
            {
                endPoints.gameObject.SetActive(true);
                rewardSpawner.gameObject.SetActive(true);
                rewardSpawner.SpawnReward(rewardIndex);
            }
        }

    }
}
