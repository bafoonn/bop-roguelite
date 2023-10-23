using System;
using System.Collections;
using UnityEngine;

namespace Pasta
{
    public class PlayerAttackHandler : MonoBehaviour
    {
        [SerializeField] private float _heavyAttackTimeMultiplier = 3.5f;
        [ReadOnly, SerializeField]
        private float _quickAttackTime = 0.2f;
        [ReadOnly, SerializeField]
        private float _heavyAttackTime = 0.8f;

        private Coroutine _attackRoutine;

        public bool CanAttack => _attackRoutine == null;
        public bool IsAttacking => _attackRoutine != null;
        private AttackArea _sensor;

        private bool _cancellable = true;

        public float _quickAttackDamage = 10f;
        public float _heavyAttackDamage = 20f;
        private Stat _damage;
        private Stat _attackSpeed;

        private AttackEffects _attackEffects;
        private bool _hasAttackEffects = false;

        private void Awake()
        {
            _sensor = GetComponentInChildren<AttackArea>();
        }

        private void Start()
        {
            _damage = StatManager.Current.GetStat(StatType.Damage);
            _damage.ValueChanged += OnDamageChanged;

            _attackSpeed = StatManager.Current.GetStat(StatType.AttackSpeed);
            _attackSpeed.ValueChanged += OnAttackSpeedChanged;
            SetDamage(_damage.Value, _attackSpeed.Value);

            _attackEffects = GetComponentInChildren<AttackEffects>();
            _hasAttackEffects = _attackEffects != null;
        }

        private void OnAttackSpeedChanged(float value)
        {
            SetDamage(_damage.Value, value);
        }

        private void OnDamageChanged(float value)
        {
            SetDamage(value, _attackSpeed.Value);
        }

        private void SetDamage(float damage, float attackSpeed)
        {
            _quickAttackTime = 1f / attackSpeed;
            _heavyAttackTime = _quickAttackTime * _heavyAttackTimeMultiplier;

            _quickAttackDamage = damage;
            _heavyAttackDamage = _quickAttackDamage * (_heavyAttackTime / _quickAttackTime) + 1;
        }

        public enum AttackType
        {
            Quick,
            Heavy
        }

        public bool TryToAttack(Vector2 dir, AttackType type = AttackType.Quick)
        {
            if (!CanAttack)
            {
                return false;
            }

            switch (type)
            {
                case AttackType.Quick:
                    _attackRoutine = StartCoroutine(QuickAttack(dir));
                    break;

                case AttackType.Heavy:
                    _attackRoutine = StartCoroutine(HeavyAttack(dir));
                    break;
            }
            return true;
        }

        private IEnumerator QuickAttack(Vector2 dir)
        {
            if (_hasAttackEffects) _attackEffects.QuickAttack();
            yield return new WaitForSeconds(_quickAttackTime);
            for (int i = 0; i < _sensor.Objects.Count; i++)
            {
                var hittable = _sensor.Objects[i];
                if (hittable == null)
                {
                    continue;
                }

                hittable.Hit(_quickAttackDamage);
            }
            
            _attackRoutine = null;
        }

        private IEnumerator HeavyAttack(Vector2 dir)
        {
            float waitTime = _heavyAttackTime * 0.5f;
            if (_hasAttackEffects) _attackEffects.AttackIndicator();
            yield return new WaitForSeconds(waitTime);

            if (_hasAttackEffects) _attackEffects.HeavyAttack();
            _cancellable = false;
            yield return new WaitForSeconds(waitTime);

            for (int i = 0; i < _sensor.Objects.Count; i++)
            {
                var hittable = _sensor.Objects[i];
                if (hittable == null)
                {
                    continue;
                }

                hittable.Hit(_heavyAttackDamage);
            }
            
            _cancellable = true;
            _attackRoutine = null;
        }

        public bool Cancel()
        {
            if (!_cancellable)
            {
                return false;
            }

            if (_attackRoutine != null)
            {
                StopCoroutine(_attackRoutine);
                _attackRoutine = null;
            }

            return true;
        }

        public void Stop()
        {
            _cancellable = true;
            Cancel();
        }
    }
}
