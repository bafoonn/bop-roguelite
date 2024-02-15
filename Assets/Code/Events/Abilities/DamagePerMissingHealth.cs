using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class DamagePerMissingHealth : ItemAbility
    {
        private StatEffect _effect = null;
        private PlayerHealth _health;

        [SerializeField] private float _maxDamageBonus = 5;
        [SerializeField] private float _bonusPerStack = 0.25f;
        [SerializeField, Range(0, 1f)] private float _maxDamageAtHealthPercentage = 0.1f;

        private float _t = 0;

        protected override void Init()
        {
            _health = _player.Health as PlayerHealth;
            _health.OnHealthChanged += OnHealthChanged;
            _item.OnAmountChanged += UpdateDamageBonus;

            _effect = new StatEffect("DamagePerHealthMissing", StatType.Damage, 0, StatEffectType.Additive);
            OnHealthChanged(_health.CurrentHealth);
        }

        private void OnDestroy()
        {
            _health.OnHealthChanged -= OnHealthChanged;
            _item.OnAmountChanged -= UpdateDamageBonus;
        }

        private void UpdateDamageBonus()
        {
            float stackBonus = _bonusPerStack * (_item.Amount - 1);
            float damageBonus = _maxDamageBonus + stackBonus;
            damageBonus *= _t;
            _effect.Update(damageBonus);
        }

        private void OnHealthChanged(float value)
        {
            float max = _health.MaxHealth;
            float min = _health.MaxHealth * _maxDamageAtHealthPercentage;

            max -= min;
            value -= min;

            _t = 1f - value / max;
            UpdateDamageBonus();
        }
    }
}
