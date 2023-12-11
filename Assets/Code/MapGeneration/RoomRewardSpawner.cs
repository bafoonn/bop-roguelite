using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class RoomRewardSpawner : MonoBehaviour
    {
        [SerializeField]
        private ItemPickup pickupPrefab = null;
        [SerializeField]
        private HealthReward healthRestore;
        [SerializeField]
        private CoinReward coin;
        //private bool firstRoom;


        public void InitializeRewardSpawn(ItemBase reward, int rewardType)
        {
            switch (rewardType)
            {
                case 0:
                    var pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
                    pickup.Setup(reward, false);
                    pickup.transform.SetParent(this.gameObject.transform);
                    break;

                case 1:
                    var healthPickup = Instantiate(healthRestore, transform.position, Quaternion.identity);
                    healthPickup.transform.SetParent(this.gameObject.transform);
                    break;

                case 2:
                    var coinPickUp = Instantiate(coin, transform.position, Quaternion.identity);
                    coinPickUp.transform.SetParent(this.gameObject.transform);
                    break;
            }
        }
    }
}
