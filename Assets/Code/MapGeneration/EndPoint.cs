using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EndPoint : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] rewardList;

        private int roomRewardIndex;

        public int GenerateRoomRewardIndex()
        {
            int random = Random.Range(0, rewardList.Length);
            return random;
        }
        public void GenerateRoomReward(int index)
        {
            roomRewardIndex = index;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                Region region = GetComponentInParent<Region>();
                region.GenerateLevel(roomRewardIndex);
            }
        }
    }
}
