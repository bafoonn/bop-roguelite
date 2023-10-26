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
        private int regionIndex = -1;
        private int startIndex;
        [SerializeField]
        private Shopkeeper shopKeeper;
        // Start is called before the first frame update
        void Start()
        {
            shopKeeper.gameObject.SetActive(false);
            for (int i = 0; i < regions.Length; i++)
            {
                regions[i].gameObject.SetActive(false);
            }
            startIndex = regionIndex;
            room = Instantiate(startRoom, transform.position, Quaternion.identity);
            room.transform.SetParent(this.gameObject.transform);
        }

        public void ChangeRegion(int roomRewardIndex)
        {
            if (regionIndex != regions.Length)
            {
                if (regionIndex != startIndex)
                {
                    regions[regionIndex].gameObject.SetActive(false);
                }
                else
                {
                    Destroy(room.gameObject);
                }

                regionIndex++;

                regions[regionIndex].gameObject.SetActive(true);
                regions[regionIndex].GenerateLevel(roomRewardIndex);
            }
            else
            {
                GameOver(true);
            }
        }

        public void GameOver(bool win)
        {
            Debug.Log("Game over");
        }
        public void ActivateShopKeeper()
        {
            shopKeeper.gameObject.SetActive(true);
        }
        public void DisableShopKeeper()
        {
            shopKeeper.gameObject.SetActive(false);
        }
    }
}
