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
        private EndPoints endPoints;

        // Start is called before the first frame update
        void Start()
        {
            levelManager = GetComponentInParent<LevelManager>();
        }
        private void Awake()
        {
            dublicateCheck = new bool[levels.Length];
        }
        public void GenerateLevel(ItemBase roomReward)
        {
            if (levelNumber != 0)
            {
                Destroy(activeLevel.gameObject);
            }
            levelNumber++;

            // Change region if level limit per region is reached
            if (levelNumber == 6)
            {
                levelManager.ChangeRegion(roomReward);
                levelNumber = 0;
            }

            // Instantiate boss level if last level for the region is reached, boss level is always the last level in the levels array
            else if (levelNumber == 5)
            {
                InstantiateLevel(levels.Length - 1, roomReward);
            }

            // Instantiate random level from levels list
            else
            {
                while(true)
                {
                    int random = Random.Range(0, levels.Length - 1);
                    if (!dublicateCheck[random])
                    {
                        InstantiateLevel(random, roomReward);
                        dublicateCheck[random] = true;
                        break;
                    }
                }
            }
        }

        private void InstantiateLevel(int levelIndex, ItemBase roomReward)
        {
            ItemAbilities.InvokeEvent(EventActionType.OnRoomEnter);
            activeLevel = Instantiate(levels[levelIndex], transform.position, Quaternion.identity, transform);          
            activeLevel.PassRewardIndex(roomReward, levelNumber);
        }

        public void ActivateShopLevel()
        {
            endPoints = GetComponentInChildren<EndPoints>();
            if (levelNumber != 4)
            {
                rewards = endPoints.GetRewards();
            }
            Destroy(activeLevel.gameObject);

            activeLevel = Instantiate(shopRoom, transform.position, Quaternion.identity, transform);

            if (levelNumber != 4)
            {
                activeLevel.ActivateEndPoints(rewards);
            }
            else
            {
                activeLevel.ActivateBossPortal();
            }

            levelManager.ActivateShopKeeper();
        }
    }
}
