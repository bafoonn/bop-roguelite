using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class HealthRestore : PickupBase
    {
        public float HealAmount = 0.1f;
        public bool IsPercentage = true;

        public static HealthRestore Spawn(HealthRestore prefab, Vector2 position, float healAmount = 0.1f, bool isPercentage = true)
        {
            var newHealthRestore = Instantiate(prefab, position, Quaternion.identity);
            newHealthRestore.HealAmount = healAmount;
            newHealthRestore.IsPercentage = isPercentage;
            return newHealthRestore;
        }

        public virtual void Heal(PlayerHealth playerHealth)
        {
            if (playerHealth.IsMaxHealth)
            {
                return;
            }

            float heal = IsPercentage ? playerHealth.MaxHealth * HealAmount : HealAmount;

            playerHealth.Heal(heal);
            Take();
        }
    }
}
