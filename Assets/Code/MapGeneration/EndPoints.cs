using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EndPoints : MonoBehaviour
    {
        [SerializeField]
        private EndPoint[] endPoints;
        private Region region;
        private int dublicateCheck;
        // Start is called before the first frame update
        void Start()
        {
            region = GetComponentInParent<Region>();

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
                    while (roomRewardIndex == dublicateCheck)
                    {
                        roomRewardIndex = endPoints[i].GenerateRoomRewardIndex();
                    }
                }

                endPoints[i].GenerateRoomReward(roomRewardIndex);
            }
        }

    }
}
