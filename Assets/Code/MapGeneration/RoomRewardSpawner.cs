using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class RoomRewardSpawner : MonoBehaviour
    {
        [SerializeField]
        private ItemPickup pickupPrefab = null;
        //private bool firstRoom;


        public void InitializeRewardSpawn(ItemBase reward)
        {
            var rewards = Items.Current.GetRewards();
            if (rewards.Count != 0)
            {
                var pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
                pickup.Setup(reward, false);
                pickup.transform.SetParent(this.gameObject.transform);

            }
        }
    }
}
