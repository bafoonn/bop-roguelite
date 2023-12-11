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
        public Stat DodgeCooldown { get; private set; }
        public Stat DodgeCount { get; private set; }

        public Stats(StatProfile profile)
        {
            Health = new Stat(profile.Health, StatType.Health, minValue: profile.MinHealth);
            Damage = new Stat(profile.Damage, StatType.Damage, minValue: profile.MinDamage);
            MovementSpeed = new Stat(profile.MovementSpeed, StatType.Movementspeed, minValue: profile.MinMovementSpeed, maxValue: profile.MaxMovementSpeed);
            AttackSpeed = new Stat(profile.AttackSpeed, StatType.Attackspeed, minValue: profile.MinAttackSpeed);
            DodgeCooldown = new Stat(profile.DodgeCooldown, StatType.DodgeCooldown, 0);
            DodgeCount = new Stat(profile.DodgeCount, StatType.DodgeCount, 1);
        }

        public void GetDPS(float coefficiency, out float damage, out float hitRate)
        {
            damage = Damage.Value - Damage.Value * coefficiency * 0.5f;
            hitRate = 1 / (AttackSpeed.Value - (AttackSpeed.Value * coefficiency * 0.5f));
        }
    }
}
