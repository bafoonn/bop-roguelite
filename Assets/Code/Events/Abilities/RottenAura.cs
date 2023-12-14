using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.VFX;

namespace Pasta
{
    public class RottenAura : ItemAbility
    {
        [SerializeField] private float _damageCoefficiency = 0.2f;
        [SerializeField] private float _baseRadius = 2.5f;
        [SerializeField] private float _radiusStackIncrease = 0.5f;
        [SerializeField, Range(0, 1f)] private float _durationStackIncreasePercentage = 0.3f;

        private CircleCollider2D _collider = null;
        private SpriteRenderer _sprite = null;

        private float _damage = 0;
        [SerializeField] private float _hitRate = 1f, _poisonDuration = 5f;
        private EnemySensor _enemySensor = null;

        private Stat _damageStat = null;

        private VisualEffect _effect = null;

        protected override void Init()
        {
            _damageStat = StatManager.Current.GetStat(StatType.Damage);
            _collider = this.AddOrGetComponent<CircleCollider2D>();
            _collider.isTrigger = true;

            _sprite = GetComponentInChildren<SpriteRenderer>();
            Assert.IsNotNull(_sprite, $"{name} couldn't find SpriteRenderer in children.");

            _effect = GetComponentInChildren<VisualEffect>();
            Assert.IsNotNull(_effect, $"{name} couldn't find VisualEffect in children.");

            _damageStat.ValueChanged += SetDamage;
            SetDamage(_damageStat.Value);
            SetRadius();
            _item.OnAmountChanged += SetRadius;
        }

        private IEnumerator Start()
        {
            _enemySensor = this.AddOrGetComponent<EnemySensor>();
            float timer = 0;
            while (true)
            {
                timer += Time.deltaTime;
                if (timer > _hitRate)
                {
                    foreach (var enemy in _enemySensor.Objects)
                    {
                        float duration = _poisonDuration + _poisonDuration * ScalingValue.GetHyperbolic(_durationStackIncreasePercentage, _item.Amount - 1);
                        enemy.Status.ApplyStatus(new PoisonStatus(_damage), duration);
                        _effect.SendEvent("Trigger");
                    }
                    timer = 0;
                }
                yield return null;
            }
        }


        private void OnDestroy()
        {
            _damageStat.ValueChanged -= SetDamage;
            _item.OnAmountChanged -= SetRadius;
        }

        private void SetDamage(float value)
        {
            _damage = value * _damageCoefficiency;
        }

        private void SetRadius()
        {
            float radius = _baseRadius + _radiusStackIncrease * _item.Amount - 1;
            _collider.radius = radius;
            _sprite.transform.localScale = Vector3.one * radius * 2;
            _effect.transform.localScale = _sprite.transform.localScale * 0.2f;
        }
    }
}
