using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class BossPortal : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            var rewards = Items.Current.GetRewards();
            int random = Random.Range(0, rewards.Count);
            ItemBase roomReward = rewards[random];
            if (col.gameObject.CompareTag("Player"))
            {
                LevelManager levelManager = GetComponentInParent<LevelManager>();
                if (GetComponentInParent<Region>())
                {
                    Region region = GetComponentInParent<Region>();
                    region.GenerateLevel(roomReward);
                }
                else
                {
                    levelManager.ChangeRegion(roomReward);
                }
                levelManager.DisableShopKeeper();
            }
        }
    }
}
