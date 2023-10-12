using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class HealPlayer : EventAction
    {
        private PlayerHealth _health;

        protected override void Setup(Player player, EventActionType type)
        {
            base.Setup(player, type);
            _health = player.GetComponent<PlayerHealth>();
        }

        protected override void Trigger(float value, bool isPercentage)
        {
            _health.Heal(isPercentage ? _health.MaxHealth * value : value);
        }
    }
}
