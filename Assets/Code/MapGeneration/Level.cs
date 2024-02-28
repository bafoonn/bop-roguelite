using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace Pasta
{
    public class Level : MonoBehaviour
    {
        private EndPoints endPoints;
        private ShopPortal shopPortal;
        private RoomRewardSpawner rewardSpawner;
        private GameObject player;
        public int enemiesLeft;
        private ItemBase reward;
        private bool isCombatRoom = true;
        private ItemBase[] rewards;
        private GameObject[] abilities;
        private EnemySpawner[] enemySpawners;
        private bool allWavesSpawned;
        private BossPortal bossPortal;
        private bool nextRoomBoss = false;
        private int levelNumber;
        public Tilemap[] tilemaps;
        public Tilemap tilemap;
        public LayerMask FloorLayer;
        private int rewardType;
        private int[] rewardTypes;
        private Trap[] traps;
        private bool isTrapLevel;
        private bool finalLevel = true;
        private FinaPortal finalDoor;

        public ItemBase Reward => reward;

        [SerializeField]
        private Transform spawnPoint;

        // Start is called before the first frame update
        void Start()
        {
            tilemaps = GetComponentsInChildren<Tilemap>(true);
            for (int i = 0; i < tilemaps.Length; i++)
            {
                if (tilemaps[i] != null)
                {
                    if (FloorLayer.Includes(tilemaps[i].gameObject.layer))
                    {
                        tilemap = tilemaps[i];
                    }
                }
            }
            if (GetComponentInChildren<EndPoints>())
            {
                endPoints = GetComponentInChildren<EndPoints>();
                endPoints.gameObject.SetActive(false);
                finalLevel = false;
            }
            else
            {
                finalDoor = GetComponentInChildren<FinaPortal>();
                finalDoor.gameObject.SetActive(false);
            }
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
            if (GetComponentInChildren<BossPortal>())
            {
                bossPortal = GetComponentInChildren<BossPortal>();
                if (!nextRoomBoss)
                {
                    bossPortal.gameObject.SetActive(false);
                }
            }
            if (GetComponentInChildren<Trap>())
            {
                traps = GetComponentsInChildren<Trap>();
                isTrapLevel = true;
            }
            player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = spawnPoint.transform.position;
            CheckIfNonCombatRoom();
        }

        private void OnEnable()
        {
            FixedEnemyAI.OnSpawn += OnEnemySpawn;
            FixedEnemyAI.OnDie += OnEnemyDeath;
        }

        private void OnEnemySpawn(FixedEnemyAI obj)
        {
            enemiesLeft += 1;
        }

        private void OnEnemyDeath(FixedEnemyAI obj)
        {
            EnemyKilled();
        }

        private void OnDisable()
        {
            FixedEnemyAI.OnSpawn -= OnEnemySpawn;
            FixedEnemyAI.OnDie -= OnEnemyDeath;
        }
        // Gets the chosen reward from last room's endpoints as a reward for clearing this room
        public void PassRewardIndex(ItemBase passedReward, int passedLevelNumber, int passedRewardType)
        {
            reward = passedReward;
            rewardType = passedRewardType;
            levelNumber = passedLevelNumber;
        }
        // Called whenever an enemy dies, at 0 enemies left activates endpoints that start the next level generation
        public void EnemyKilled()
        {
            enemiesLeft--;
            if (enemiesLeft <= 0)
            {
                allWavesSpawned = true;
                enemySpawners = GetComponentsInChildren<EnemySpawner>();
                for (int i = 0; i < enemySpawners.Length; i++)
                {
                    if (enemySpawners[i].wavesCheck() != 0)
                    {
                        allWavesSpawned = false;
                    }
                }
                if (!allWavesSpawned)
                {
                    for (int i = 0; i < enemySpawners.Length; i++)
                    {
                        enemySpawners[i].startNewWave();
                    }
                }
                else
                {
                    abilities = GameObject.FindGameObjectsWithTag("Ability");
                    foreach (var ability in abilities)
                    {
                        Destroy(ability.gameObject);
                    }
                    if (isTrapLevel)
                    {
                        for (int i = 0; i < traps.Length; i++)
                        {
                            traps[i].Disable();
                        }
                    }
                    if (!finalLevel)
                    {
                        rewardSpawner.gameObject.SetActive(true);
                        rewardSpawner.InitializeRewardSpawn(reward, rewardType);
                    }
                    else
                    {
                        finalDoor.gameObject.SetActive(true);
                    }
                }
            }
        }
        // Method for enemies that spawn minions etc.
        public EnemyAi SpawnEnemy(EnemyAi enemyPrefab, Vector2 position)
        {
            Assert.IsNotNull(enemyPrefab, "Tried to spawn null enemyPrefab: " + nameof(enemyPrefab));
            return Instantiate(enemyPrefab, position, Quaternion.identity, transform);
        }

        // Called when the player picks up the room reward
        // Activates the objects needed to advance to the next level
        public void PickedUpReward()
        {
            if (levelNumber != 4)
            {
                if (!finalLevel)
                {
                    endPoints.gameObject.SetActive(true);
                    endPoints.GenerateRoomRewards();
                }
            }
            else
            {
                if (!finalLevel)
                {
                    shopPortal.gameObject.SetActive(true);
                }
            }
        }
        public void ActivateEndPoints(ItemBase[] passedRewards, int[] passedRewardTypes)
        {
            rewards = passedRewards;
            rewardTypes = passedRewardTypes;
            isCombatRoom = false;
        }

 
        private void CheckIfNonCombatRoom()
        {
            if (!isCombatRoom)
            {
                endPoints.gameObject.SetActive(true);
                endPoints.PassRewards(rewards, rewardTypes);
            }
        }
        public void ActivateBossPortal()
        {
            nextRoomBoss = true;
        }
    }
}
