using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField]
        private StartRoom startRoom;
        private StartRoom room;
        [SerializeField]
        private Region[] regions;
        private Region activeRegion;
        private int regionIndex = -1;
        private int startIndex;
        [SerializeField]
        private Shopkeeper shopKeeper;
        private Shopkeeper activeShopKeeper;

        public Region ActiveRegion => activeRegion;
        public Level ActiveLevel => activeRegion.ActiveLevel;

        public override bool PersistSceneLoad => false;

        // Start is called before the first frame update
        void Start()
        {
            activeShopKeeper = Instantiate(shopKeeper, transform.position, Quaternion.identity);
            activeShopKeeper.gameObject.SetActive(false);
            startIndex = regionIndex;
            room = Instantiate(startRoom, transform.position, Quaternion.identity, transform);
        }

        // Is called at the start to load the first region and then after beating and exiting a boss room to load the next region
        public void ChangeRegion(ItemBase roomReward, int rewardType)
        {
            if (regionIndex != regions.Length)
            {
                if (regionIndex != startIndex)
                {
                    Destroy(activeRegion.gameObject);
                }
                else
                {
                    Destroy(room.gameObject);
                }

                regionIndex++;

                activeRegion = Instantiate(regions[regionIndex], transform.position, Quaternion.identity, transform);
                activeRegion.GenerateLevel(roomReward, rewardType);
            }
            else
            {
                GameOver(true);
            }
        }

        // Called when either the player dies or beats the boss of the final region
        public void GameOver(bool win)
        {
            if (win)
            {
                Debug.Log("Game over! You win!");
            }
            else
            {
                Debug.Log("Game over! You lose!");
            }
        }

        //Shopkeeper is a separate game object that's activated whenever the player enters into a shop room
        public void ActivateShopKeeper()
        {
            activeShopKeeper.gameObject.SetActive(true);
            activeShopKeeper.GenerateItems();
        }
        public void DisableShopKeeper()
        {
            activeShopKeeper.gameObject.SetActive(false);
        }
    }
}
