using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class HealContext : EventContext
    {
        public PlayerHealth Health;
        public HealContext(PlayerHealth health) : base(EventActionType.OnHeal)
        {
            Health = health;
        }
    }
}
