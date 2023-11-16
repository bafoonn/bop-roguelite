using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class RottenAura : EventAction
    {
        [SerializeField] private float _damageCoefficiency = 0.2f;
        [SerializeField] private float _radius = 2.5f;

        private CircleCollider2D _collider = null;

        private float _damage = 0;
        [SerializeField] private float _hitRate = 1f, _poisonDuration = 5f;
        private EnemySensor _enemySensor = null;

        private Stat _damageStat = null;

        protected override void Init()
        {
            base.Init();
            _damageStat = StatManager.Current.GetStat(StatType.Damage);
            _collider = this.AddOrGetComponent<CircleCollider2D>();
            _collider.radius = _radius;

            OnDamageChanged(_damageStat.Value);

            var sprite = GetComponentInChildren<SpriteRenderer>();
            if (sprite != null)
            {
                sprite.transform.localScale = Vector3.one * _radius * 2;
            }

            _damageStat.ValueChanged += OnDamageChanged;
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
                        enemy.Status.ApplyStatus(new PoisonStatus(_damage), _poisonDuration);
                    }
                    timer = 0;
                }
                yield return null;
            }
        }


        private void OnDestroy()
        {
            _damageStat.ValueChanged -= OnDamageChanged;
        }

        private void OnDamageChanged(float value)
        {
            _damage = value * _damageCoefficiency;
        }
    }
}
