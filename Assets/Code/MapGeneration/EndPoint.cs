using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EndPoint : MonoBehaviour
    {
        [SerializeField]
        private ItemBase[] rewardList;

        private int roomRewardIndex;
        [SerializeField] SpriteRenderer itemDisplay;

        private bool isActive = false;
        private float timer = 0.5f;

        private void Update()
        {
            // Small time window to prevent the player from activating an endpoint the moment they are spawned
            if (timer <= 0)
            {
                isActive = true;
            }

            else
            {
                timer -= Time.deltaTime;
            }
        }

        // Called for an initial reward generation that has to pass a dublicate check
        public int GenerateRoomRewardIndex()
        {
            var rewards = Items.Current.GetRewards();
            int random = Random.Range(0, rewards.Count);
            return random;
        }

        // Called when the index has passed the dublicate check and will be assigned as the reward for the next room if the player activates this endpoint
        public void GenerateRoomReward(int index)
        {
            var rewards = Items.Current.GetRewards();
            roomRewardIndex = index;
            itemDisplay.sprite = rewards[index].Sprite;
        }

        // When player collides with an endpoint, level generation is called and the reward index for this endpoint is passed to the next level as its reward
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && isActive)
            {
                if (GetComponentInParent<Region>())
                {
                    Region region = GetComponentInParent<Region>();
                    region.GenerateLevel(roomRewardIndex);
                }
                else
                {
                    LevelManager levelManager = GetComponentInParent<LevelManager>();
                    levelManager.ChangeRegion(roomRewardIndex);
                }
            }
        }

        // Keeps the player from activating the exit in case the player is inside the endpoint when they are spawned
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && !isActive)
            {
                isActive = true;
            }
        }

    }
}
