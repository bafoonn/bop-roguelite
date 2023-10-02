using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EndPoints : MonoBehaviour
    {
        [SerializeField]
        private EndPoint[] endPoints;
        private int dublicateCheck;
        private int loopStopper = 10;
        private int loop;
        // Start is called before the first frame update
        void Start()
        {
            // Dublicate check for room rewards in order to have each endpoint offer a different reward
            for (int i = 0; i < endPoints.Length; i++)
            {
                int roomRewardIndex = endPoints[i].GenerateRoomRewardIndex();

                if (i == 0)
                {
                    dublicateCheck = roomRewardIndex;
                }
                else
                {
                    // Loops until a non dublicate reward is found. 'Or' statement is there to prevent infinite loops
                    while (roomRewardIndex == dublicateCheck || loop < loopStopper)
                    {
                        roomRewardIndex = endPoints[i].GenerateRoomRewardIndex();
                        loop++;
                    }
                }

                endPoints[i].GenerateRoomReward(roomRewardIndex);
            }
        }
    }
}
