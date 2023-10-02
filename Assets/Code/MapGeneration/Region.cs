using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Region : MonoBehaviour
    {
        [SerializeField]
        private Level[] levels;

        private int levelNumber = 0;

        private LevelManager levelManager;
        private Level activeLevel;

        // Start is called before the first frame update
        void Start()
        {
            levelManager = GetComponentInParent<LevelManager>();
        }
        public void GenerateLevel(int roomRewardIndex)
        {
            if (levelNumber != 0)
            {
                Destroy(activeLevel.gameObject);
            }
            levelNumber++;

            // Change region if level limit per region is reached
            if (levelNumber == 6)
            {
                levelManager.ChangeRegion();
                levelNumber = 0;
            }

            // Instantiate boss level if last level for the region is reached, boss level is always the last level in the levels list
            else if (levelNumber == 5)
            {
                InstantiateLevel(levels.Length - 1, roomRewardIndex);
            }

            // Instantiate random level from levels list
            else
            {
                int random = Random.Range(0, levels.Length - 1);
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
