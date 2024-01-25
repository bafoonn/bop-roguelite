using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EndPoint : Trap
    {
        private ItemBase roomReward;
        private ItemBase returnedItem;
        [SerializeField] HealthRestore healthRestore;
        [SerializeField] Coin coinPickUp;
        private int rewardType;
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

        public int GenerateRoomRewardType()
        {
            int random = Random.Range(0, 3);
            rewardType = random;
            return rewardType;
        }
        public void OnlyItemRewards(ItemBase reward)
        {
            rewardType = 0;
            GenerateRoomReward(reward, rewardType);
        }

        // Called for an initial reward generation that has to pass a dublicate check
        public ItemBase GenerateRoomRewardIndex()
        {
            var level = GetComponentInParent<Level>(includeInactive: true);
            var rewards = Items.Current.GetRewards();

            if (level == null)
            {
                int random = Random.Range(0, rewards.Count);
                return rewards[random];
            }

            int index = -1;

            int loop = 0;
            while (index < 0)
            {
                int random = Random.Range(0, rewards.Count);
                ItemBase randomItem = rewards[random];

                if (randomItem != level.Reward)
                {
                    returnedItem = randomItem;
                }

                loop++;
                if (loop >= rewards.Count)
                {
                    index = 0;
                }
            }

            return returnedItem;
        }

        // Called when the reward has passed the dublicate check and will be assigned as the reward for the next room if the player activates this endpoint
        public void GenerateRoomReward(ItemBase reward, int rewardType)
        {
            switch (rewardType)
            {
                case 0:
                    var rewards = Items.Current.GetRewards();
                    roomReward = reward;
                    itemDisplay.sprite = reward.Sprite;
                    break;
                case 1:
                    itemDisplay.sprite = healthRestore.GetComponent<SpriteRenderer>().sprite;
                    break;
                case 2:
                    itemDisplay.sprite = coinPickUp.GetComponent<SpriteRenderer>().sprite;
                    break;
            }
        }

        // When player collides with an endpoint, level generation is called and the reward for this endpoint is passed to the next level as its reward
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && isActive)
            {
                LevelManager levelManager = GetComponentInParent<LevelManager>();
                if (GetComponentInParent<Region>())
                {
                    Region region = GetComponentInParent<Region>();
                    region.GenerateLevel(roomReward, rewardType);
                }
                else
                {
                    levelManager.ChangeRegion(roomReward, rewardType);
                }
                levelManager.DisableShopKeeper();
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
