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

        public override bool DoPersist => false;

        public Stat GetStat(StatType type)
        {
            Assert.IsNotNull(_currentStatProfile, "CurrentStatProfile is null.");
            Stat stat;
            switch (type)
            {
                case StatType.Health:
                    stat = _currentStatProfile.Health;
                    break;
                case StatType.Damage:
                    stat = _currentStatProfile.Damage;
                    break;
                case StatType.AttackSpeed:
                    stat = _currentStatProfile.AttackSpeed;
                    break;
                case StatType.MovementSpeed:
                    stat = _currentStatProfile.MovementSpeed;
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

        private void OnApplicationQuit()
        {

        }
    }
}
