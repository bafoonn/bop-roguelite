using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Region : MonoBehaviour
    {
        [SerializeField]
        private Level[] levels;

        private int levelIndex = 0;

        private LevelManager levelManager;
        private EndPoints endPoints;
        private Level activeLevel;
        private GameObject[] rewardList;

        // Start is called before the first frame update
        void Start()
        {
            levelManager = GetComponentInParent<LevelManager>();
        }
        public void GenerateLevel(int roomRewardIndex)
        {
            if (levelIndex != 0)
            {
                Destroy(activeLevel.gameObject);
            }
            levelIndex++;
            // Change region
            if (levelIndex == 6)
            {
                levelManager.ChangeRegion();
                levelIndex = 0;
            }
            // Instantiate boss level, boss level is always the last level in the levels list
            else if (levelIndex == 5)
            {
                InstantiateLevel(levels.Length, roomRewardIndex);
            }
            // Instantiate random level from levels list
            else
            {
                int random = Random.Range(0, levels.Length-1);
                InstantiateLevel(random, roomRewardIndex);
            }
        }
        
        private void InstantiateLevel(int levelIndex, int roomRewardIndex)
        {
            activeLevel = Instantiate(levels[levelIndex], transform.position, Quaternion.identity);
            activeLevel.transform.SetParent(this.gameObject.transform);
            activeLevel.PassRewardIndex(roomRewardIndex);
        }
    }
}
