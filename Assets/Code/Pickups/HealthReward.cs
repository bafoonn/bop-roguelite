using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class HealthReward : HealthRestore
    {
        private Level level;
        // Start is called before the first frame update
        void Start()
        {
            level = GetComponentInParent<Level>();
        }

        public override void Heal(PlayerHealth playerHealth)
        {
            float heal = IsPercentage ? playerHealth.MaxHealth * HealAmount : HealAmount;

            playerHealth.Heal(heal);
            Take();
        }
        public override void Take()
        {
            base.Take();
            if (level != null)
            {
                level.PickedUpReward();
            }
        }
    }
}
