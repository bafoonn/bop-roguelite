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

            // Instantiate boss level if last level for the region is reached, boss level is always the last level in the levels list
            else if (levelNumber == 5)
            {
                InstantiateLevel(levels.Length - 1, roomReward);
            }

            // Instantiate random level from levels list
            else
            {
                var rewards = Items.Current.GetRewards();
                int random = Random.Range(0, levels.Length - 1);
                InstantiateLevel(random, roomReward);
            }
        }

        private void InstantiateLevel(int levelIndex, ItemBase roomRewardIndex)
        {
            EventActions.InvokeEvent(EventActionType.OnRoomEnter);
            activeLevel = Instantiate(levels[levelIndex], transform.position, Quaternion.identity, transform);
            activeLevel.PassRewardIndex(roomRewardIndex);
        }

        public void ActivateShopLevel()
        {
            endPoints = GetComponentInChildren<EndPoints>();
            rewards = endPoints.GetRewards();
            Destroy(activeLevel.gameObject);

            activeLevel = Instantiate(shopRoom, transform.position, Quaternion.identity, transform);
            activeLevel.ActivateEndPoints(rewards);

            levelManager.ActivateShopKeeper();
        }
    }
}
