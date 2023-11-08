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
            Stat stat;
            switch (type)
            {
                case StatType.Health:
                    stat = _currentStats.Health;
                    break;
                case StatType.Damage:
                    stat = _currentStats.Damage;
                    break;
                case StatType.Attackspeed:
                    stat = _currentStats.AttackSpeed;
                    break;
                case StatType.Movementspeed:
                    stat = _currentStats.MovementSpeed;
                    break;
                default:
                    throw new NotImplementedException($"GetStat of type {type} is not implemented.");
            }
            return stat;
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
