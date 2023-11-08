using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Stats
    {
        public Stat Health { get; private set; }
        public Stat Damage { get; private set; }
        public Stat MovementSpeed { get; private set; }
        public Stat AttackSpeed { get; private set; }

        public Stats(StatProfile profile)
        {
            Health = new Stat(profile.Health, StatType.Health);
            Damage = new Stat(profile.Damage, StatType.Damage);
            MovementSpeed = new Stat(profile.MovementSpeed, StatType.Movementspeed, profile.MaximumMovementSpeed);
            AttackSpeed = new Stat(profile.AttackSpeed, StatType.Attackspeed);
        }

        public void GetDPS(float coefficiency, out float damage, out float hitRate)
        {
            damage = Damage.Value - Damage.Value * coefficiency * 0.5f;
            hitRate = 1 / (AttackSpeed.Value - (AttackSpeed.Value * coefficiency * 0.5f));
        }
    }
}
