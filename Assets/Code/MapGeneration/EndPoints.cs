using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EndPoints : MonoBehaviour
    {
        [SerializeField]
        private EndPoint[] endPoints;
        private ItemBase[] rewards;
        private ItemBase dublicateCheck;
        private int loopStopper = 10;
        private int loop;
        public void GenerateRoomRewards()
        {
            rewards = new ItemBase[endPoints.Length];
            // Dublicate check for room rewards in order to have each endpoint offer a different reward
            for (int i = 0; i < endPoints.Length; i++)
            {
                ItemBase roomReward = endPoints[i].GenerateRoomRewardIndex();

                if (i == 0)
                {
                    dublicateCheck = roomReward;
                }
                else
                {
                    // Loops until a non dublicate reward is found. 'Or' statement is there to prevent infinite loops
                    while (roomReward == dublicateCheck || loop < loopStopper)
                    {
                        roomReward = endPoints[i].GenerateRoomRewardIndex();
                        loop++;
                    }
                    loop = 0;
                }
                rewards[i] = roomReward;
                endPoints[i].GenerateRoomReward(roomReward);
            }
        }
        public ItemBase[] GetRewards()
        {
            return rewards;
        }
        public void PassRewards(ItemBase[] rewards)
        {
            for (int i = 0; i < endPoints.Length; i++)
            {
                endPoints[i].GenerateRoomReward(rewards[i]);
            }
        }
    }
}
