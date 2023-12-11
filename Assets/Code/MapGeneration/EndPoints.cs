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
        private int intDublicateCheck = 5;
        private int loopStopper = 10;
        private int loop;
        private int rewardType = 0;
        private int[] rewardTypes;
        public void GenerateRoomRewards()
        {
            rewards = new ItemBase[endPoints.Length];
            rewardTypes = new int[endPoints.Length];
            // Dublicate check for room rewards in order to have each endpoint offer a different reward
            for (int i = 0; i < endPoints.Length; i++)
            {
                rewardType = endPoints[i].GenerateRoomRewardType();
                ItemBase roomReward = endPoints[i].GenerateRoomRewardIndex();

                if (i == 0)
                {
                    dublicateCheck = roomReward;
                    if (rewardType != 0)
                    {
                        intDublicateCheck = rewardType;
                    }
                }
                else
                {
                    while (true)
                    {
                        rewardType = endPoints[i].GenerateRoomRewardType();
                        if (rewardType != intDublicateCheck)
                        {
                            break;
                        }
                    }

                    // Loops until a non dublicate reward is found. 'Or' statement is there to prevent infinite loops
                    while (true)
                    {
                        roomReward = endPoints[i].GenerateRoomRewardIndex();
                        if (roomReward != dublicateCheck)
                        {
                            break;
                        }
                    }
                }
                rewards[i] = roomReward;
                rewardTypes[i] = rewardType;
                endPoints[i].GenerateRoomReward(roomReward, rewardType);
            }
        }
        public ItemBase[] GetRewards()
        {
            return rewards;
        }
        public int[] GetRewardTypes()
        {
            return rewardTypes;
        }
        public void PassRewards(ItemBase[] rewards, int[] passedRewardType)
        {
            for (int i = 0; i < endPoints.Length; i++)
            {
                endPoints[i].GenerateRoomReward(rewards[i], passedRewardType[i]);
            }
        }
        public void OnlyItemRewards()
        {
            for (int i = 0; i < endPoints.Length; i++)
            {
                endPoints[i].OnlyItemRewards(rewards[i]);
            }
        }
    }
}
