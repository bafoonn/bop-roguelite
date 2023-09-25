using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class RoomRewardSpawner : MonoBehaviour
    {
        [SerializeField]
        private Item[] rewards;
        [SerializeField]
        private Pickup pickupPrefab = null;


        public void SpawnReward(int rewardIndex)
        {
            if (rewards.Length != 0)
            {
                var pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
                pickup.Setup(rewards[rewardIndex]);
                pickup.transform.SetParent(this.gameObject.transform);
            }
        }
    }
}
