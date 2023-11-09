using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class LevelManager : MonoBehaviour
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
        // Start is called before the first frame update
        void Start()
        {
            activeShopKeeper = Instantiate(shopKeeper, transform.position, Quaternion.identity);
            activeShopKeeper.gameObject.SetActive(false);
            startIndex = regionIndex;
            room = Instantiate(startRoom, transform.position, Quaternion.identity, transform);
        }

        public void ChangeRegion(ItemBase roomReward)
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
                activeRegion.GenerateLevel(roomReward);
            }
            else
            {
                GameOver(true);
            }
        }

        public void GameOver(bool win)
        {
            if (win)
            {
                Debug.Log("Game over! You win!");
            }
            else
            {
                Debug.Log("Game over! You lose!p");
            }
        }
        public void ActivateShopKeeper()
        {
            activeShopKeeper.gameObject.SetActive(true);
        }
        public void DisableShopKeeper()
        {
            activeShopKeeper.gameObject.SetActive(false);
        }
    }
}
