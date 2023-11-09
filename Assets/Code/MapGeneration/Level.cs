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
        private ItemBase reward;
        private bool isCombatRoom = true;
        private ItemBase[] rewards;
        private GameObject[] abilities;

        public ItemBase Reward => reward;

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

        public void PassRewardIndex(ItemBase passedReward)
        {
            reward = passedReward;
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
                abilities = GameObject.FindGameObjectsWithTag("Ability");
                foreach (var ability in abilities)
                {
                    Destroy(ability.gameObject);
                }
                rewardSpawner.gameObject.SetActive(true);
                rewardSpawner.InitializeRewardSpawn(reward);
            }
        }
        public void PickedUpReward()
        {
            endPoints.gameObject.SetActive(true);
            endPoints.GenerateRoomRewards();
            shopPortal.gameObject.SetActive(true);
        }
        public void ActivateEndPoints(ItemBase[] passedRewards)
        {
            rewards = passedRewards;
            isCombatRoom = false;
        }
        private void CheckIfNonCombatRoom()
        {
            if (!isCombatRoom)
            {
                endPoints.gameObject.SetActive(true);
                endPoints.PassRewards(rewards);
            }
        }
    }
}
