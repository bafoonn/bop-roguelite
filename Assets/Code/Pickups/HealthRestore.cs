using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class HealthRestore : PickupBase
    {
        public float HealPercentage = 0.1f;

        public void Heal(PlayerHealth playerHealth)
        {
            playerHealth.Heal(playerHealth.MaxHealth * HealPercentage);
            Take();
        }
    }
}
