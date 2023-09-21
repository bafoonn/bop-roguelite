using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class RoomRewardSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] rewards;

         
        public void SpawnReward(int rewardIndex)
        {
            if (rewards.Length != 0)
            {
                Instantiate(rewards[rewardIndex], transform.position, Quaternion.identity);
            }
        }
    }
}
