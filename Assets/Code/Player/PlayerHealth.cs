using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PlayerHealth : Health
    {
        private Stat _healthStat;

        private void Start()
        {
            _healthStat = StatManager.Current.GetStat(StatType.Health);
            _healthStat.ValueChanged += OnMaxHealthChanged;
            OnMaxHealthChanged(_healthStat.Value);
        }

        private void OnDestroy()
        {
            _healthStat.ValueChanged -= OnMaxHealthChanged;
        }

        private void OnMaxHealthChanged(float value)
        {
            SetMax(value);
        }

        public override void Heal(float amount)
        {
            base.Heal(amount);
        }
    }
}
