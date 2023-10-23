using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Level : MonoBehaviour
    {
        private EndPoints endPoints;
        private ShopPortal shopPortal;
        private RoomRewardSpawner rewardSpawner;
        private GameObject player;
        private int enemiesLeft;
        private int rewardIndex;
        private bool isCombatRoom = true;
        private int[] indexes;

        public int RewardIndex => rewardIndex;

        [SerializeField]
        private Transform spawnPoint;

        // Start is called before the first frame update
        void Start()
        {
            endPoints = GetComponentInChildren<EndPoints>();
            endPoints.gameObject.SetActive(false);
            if (GetComponentInChildren<ShopPortal>())
            {
                shopPortal = GetComponentInChildren<ShopPortal>();
                shopPortal.gameObject.SetActive(false);
            }
            if (GetComponentInChildren<RoomRewardSpawner>())
            {
                rewardSpawner = GetComponentInChildren<RoomRewardSpawner>();
                rewardSpawner.gameObject.SetActive(false);
            }
            player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = spawnPoint.transform.position;
            CheckIfNonCombatRoom();
        }

        public void PassRewardIndex(int index)
        {
            rewardIndex = index;
        }
        // Each enemy spawner passes the value of the number of enemies it will spawn, which is then added to the total sum
        public void AddToEnemyCount(int addedEnemyCount)
        {
            enemiesLeft += addedEnemyCount;
        }
        // Called whenever an enemy dies, at 0 enemies left activates endpoints that start the next level generation
        public void EnemyKilled()
        {
            enemiesLeft--;
            if (enemiesLeft <= 0)
            {
                endPoints.gameObject.SetActive(true);
                endPoints.GenerateRoomRewards();
                rewardSpawner.gameObject.SetActive(true);
                rewardSpawner.InitializeRewardSpawn(rewardIndex);
                shopPortal.gameObject.SetActive(true);
            }
        }
        public void ActivateEndPoints(int[] rewardIndexes)
        {
            indexes = rewardIndexes;
            isCombatRoom = false;
        }
        private void CheckIfNonCombatRoom()
        {
            if (!isCombatRoom)
            {
                endPoints.gameObject.SetActive(true);
                endPoints.PassRewardIndexes(indexes);
            }
        }
    }
}
