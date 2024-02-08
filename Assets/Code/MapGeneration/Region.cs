using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Region : MonoBehaviour
    {
        [SerializeField]
        private Level shopRoom;
        [SerializeField]
        private Level[] levels;
        private bool[] dublicateCheck;

        private int levelNumber = 0;

        private LevelManager levelManager;
        private Level activeLevel;

        private ItemBase[] rewards;
        private int[] rewardTypes;
        private EndPoints endPoints;

        public Level ActiveLevel => activeLevel;

        // Start is called before the first frame update
        void Start()
        {
            levelManager = GetComponentInParent<LevelManager>();
        }
        private void Awake()
        {
            dublicateCheck = new bool[levels.Length];
        }
        public void GenerateLevel(ItemBase roomReward, int rewardType)
        {
            // Clear the previous level
            if (levelNumber != 0)
            {
                Destroy(activeLevel.gameObject);
            }
            levelNumber++;

            // Change region if level limit per region is reached
            if (levelNumber == 6)
            {
                levelManager.ChangeRegion(roomReward, rewardType);
                levelNumber = 0;
            }

            // Instantiate boss level if last level for the region is reached, boss level is always the last level in the levels array
            else if (levelNumber == 5)
            {
                InstantiateLevel(levels.Length - 1, roomReward, rewardType);
            }

            // If no special level is to be activated, instantiate random level from levels list
            else
            {
                while (true)
                {
                    int random = Random.Range(0, levels.Length - 1);
                    if (!dublicateCheck[random])
                    {
                        InstantiateLevel(random, roomReward, rewardType);
                        dublicateCheck[random] = true;
                        break;
                    }
                }
            }
        }

        private void InstantiateLevel(int levelIndex, ItemBase roomReward, int rewardType)
        {
            ItemAbilities.InvokeEvent(EventActionType.OnRoomEnter);
            activeLevel = Instantiate(levels[levelIndex], transform.position, Quaternion.identity, transform);
            activeLevel.PassRewardIndex(roomReward, levelNumber, rewardType);
        }

        // Shop level is a non-combat room that doesn't add to the level count
        // Always instantiated as the final room before a boss room
        public void ActivateShopLevel()
        {
            endPoints = GetComponentInChildren<EndPoints>();
            if (levelNumber != 4)
            {
                rewards = endPoints.GetRewards();
                rewardTypes = endPoints.GetRewardTypes();

            }
            Destroy(activeLevel.gameObject);

            activeLevel = Instantiate(shopRoom, transform.position, Quaternion.identity, transform);

            if (levelNumber != 4)
            {
                activeLevel.ActivateEndPoints(rewards, rewardTypes);
            }
            else
            {
                activeLevel.ActivateBossPortal();
            }

            levelManager.ActivateShopKeeper();
        }
    }
}
