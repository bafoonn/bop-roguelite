using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Stats
    {
        public Stat Health;
        public Stat Damage;
        public Stat MovementSpeed;
        public Stat AttackSpeed;

        public Stats(StatProfile profile)
        {
            Health = new Stat(profile.Health, StatType.Health);
            Damage = new Stat(profile.Damage, StatType.Damage);
            MovementSpeed = new Stat(profile.MovementSpeed, StatType.MovementSpeed);
            AttackSpeed = new Stat(profile.AttackSpeed, StatType.AttackSpeed);
        }
    }
}
