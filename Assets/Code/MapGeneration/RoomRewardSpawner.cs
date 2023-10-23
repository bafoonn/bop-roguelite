using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class RoomRewardSpawner : MonoBehaviour
    {
        [SerializeField]
        private Pickup pickupPrefab = null;
        //private bool firstRoom;


        public void InitializeRewardSpawn(int rewardIndex)
        {
            var rewards = Items.Current.GetRewards();
            if (rewards.Count != 0)
            {
                var pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
                pickup.Setup(rewards[rewardIndex], false);
                pickup.transform.SetParent(this.gameObject.transform);


                /*
                if (firstRoom)
                {
                    int random = Random.Range(0, rewards.Length);
                    SpawnReward(random);
                    firstRoom = false;
                }
                else
                {
                    SpawnReward(rewardIndex);
                }
                */
            }
        }
        /*
        private void SpawnReward(int rewardIndex)
        {
            var pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
            pickup.Setup(rewards[rewardIndex]);
            pickup.transform.SetParent(this.gameObject.transform);
        }
        */
    }
}
