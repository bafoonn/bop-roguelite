using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class RoomRewardSpawner : MonoBehaviour
    {
        [SerializeField]
        private ItemBase[] rewards;
        [SerializeField]
        private Pickup pickupPrefab = null;
        private bool firstRoom;


        public void InitializeRewardSpawn(int rewardIndex)
        {
            if (rewards.Length != 0)
            {
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
            }
        }

        private void SpawnReward(int rewardIndex)
        {
            var pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
            pickup.Setup(rewards[rewardIndex]);
            pickup.transform.SetParent(this.gameObject.transform);
        }
    }
}
