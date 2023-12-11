using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pasta
{
    public class StatManager : Singleton<StatManager>
    {
        [SerializeField] private StatProfile _currentStatProfile;
        private Stats _currentStats;

        public override bool DoPersist => false;
        public Stats Stats => _currentStats;

        protected override void Init()
        {
            if (_currentStatProfile == null)
            {
                return;
            }

            _currentStats = new Stats(_currentStatProfile);
        }

        public Stat GetStat(StatType type)
        {
            Assert.IsNotNull(_currentStatProfile, "CurrentStatProfile is null.");
            switch (type)
            {
                case StatType.Health: return _currentStats.Health;
                case StatType.Damage: return _currentStats.Damage;
                case StatType.Attackspeed: return _currentStats.AttackSpeed;
                case StatType.Movementspeed: return _currentStats.MovementSpeed;
                case StatType.DodgeCount: return _currentStats.DodgeCount;
                case StatType.DodgeCooldown: return _currentStats.DodgeCooldown;
                default: throw new NotImplementedException($"GetStat of type {type} is not implemented.");
            }
        }

        public bool AddEffectToStat(StatEffect effect)
        {
            var stat = GetStat(effect.Stat);
            return stat.AddEffect(effect);
        }

        public bool RemoveEffectFromStat(StatEffect effect)
        {
            var stat = GetStat(effect.Stat);
            return stat.RemoveEffect(effect);
        }
    }
}
