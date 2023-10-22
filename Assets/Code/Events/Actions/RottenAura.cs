using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class RottenAura : EventAction
    {
        [SerializeField] private float _damageCoefficiency = 0.2f;
        [SerializeField] private float _radius = 2.5f;

        private DamageArea _damageArea = null;
        private CircleCollider2D _collider = null;

        private Stat _damageStat = null;
        private Stat _attackSpeedStat = null;

        protected override void Init()
        {
            base.Init();
            _damageStat = StatManager.Current.GetStat(StatType.Damage);
            _attackSpeedStat = StatManager.Current.GetStat(StatType.AttackSpeed);
            _collider = this.AddOrGetComponent<CircleCollider2D>();
            _collider.radius = _radius;
            _damageArea = this.AddOrGetComponent<DamageArea>();
            _damageArea.Damage = _damageStat.Value * _damageCoefficiency;
            _damageArea.Interval = 1 / (_attackSpeedStat.Value * _damageCoefficiency);
            _damageArea.SensedLayers = 1 << LayerMask.NameToLayer("Enemy");
            var sprite = GetComponentInChildren<SpriteRenderer>();
            if (sprite != null)
            {
                sprite.transform.localScale = Vector3.one * _radius * 2;
            }

            _damageStat.ValueChanged += OnDamageChanged;
            _attackSpeedStat.ValueChanged += OnAttackSpeedChanged;
        }


        private void OnDestroy()
        {
            _damageStat.ValueChanged -= OnDamageChanged;
        }

        private void OnDamageChanged(float value)
        {
            _damageArea.Damage = value * _damageCoefficiency;
        }

        private void OnAttackSpeedChanged(float value)
        {
            _damageArea.Interval = 1 / (value * _damageCoefficiency);
        }
    }
}
