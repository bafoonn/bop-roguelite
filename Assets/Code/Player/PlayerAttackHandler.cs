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
            _attackEffects = GetComponentInChildren<AttackEffects>();
        }

        private void Start()
        {
            _damage = StatManager.Current.GetStat(StatType.Damage);
            _damage.ValueChanged += OnDamageChanged;

            _attackSpeed = StatManager.Current.GetStat(StatType.Attackspeed);
            _attackSpeed.ValueChanged += OnAttackSpeedChanged;
            SetDamage(_damage.Value, _attackSpeed.Value);

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
            _attackEffects.SetIndicatorLifetime(_heavyAttackTime);

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

            Attack(dir, type);

            return true;

        }

        public void Attack(Vector2 dir, AttackType type = AttackType.Quick)
        {
            switch (type)
            {
                case AttackType.Quick:
                    _attackRoutine = StartCoroutine(QuickAttack());
                    break;

                case AttackType.Heavy:
                    _attackRoutine = StartCoroutine(HeavyAttack());
                    break;
            }
        }

        private IEnumerator QuickAttack()
        {
            if (_hasAttackEffects) _attackEffects.QuickAttack();
            HitObjects(AttackType.Quick);
            yield return new WaitForSeconds(_quickAttackTime);
            _attackRoutine = null;
        }

        private IEnumerator HeavyAttack()
        {
            float waitTime = _heavyAttackTime * 0.5f;
            if (_hasAttackEffects) _attackEffects.AttackIndicator();
            yield return new WaitForSeconds(waitTime);

            _cancellable = false;
            yield return new WaitForSeconds(waitTime);
            if (_hasAttackEffects) _attackEffects.HeavyAttack();

            HitObjects(AttackType.Heavy);

            _cancellable = true;
            _attackRoutine = null;
        }

        private void HitObjects(AttackType type)
        {

            for (int i = 0; i < _sensor.Objects.Count; i++)
            {
                var hittable = _sensor.Objects[i];
                if (hittable == null)
                {
                    continue;
                }

                float damage = 0;
                switch (type)
                {
                    case AttackType.Quick:
                        damage = _quickAttackDamage;
                        EventActions.InvokeEvent(new HitContext(hittable, damage, EventActionType.OnQuickHit));
                        break;
                    case AttackType.Heavy:
                        damage = _heavyAttackDamage;
                        EventActions.InvokeEvent(new HitContext(hittable, damage, EventActionType.OnHeavyHit));
                        break;
                }

                EventActions.InvokeEvent(new HitContext(hittable, damage, EventActionType.OnHit));
                hittable.Hit(damage);
            }
        }

        public bool Cancel()
        {
            if (!_cancellable)
            {
                return false;
            }

            if (_attackRoutine != null)
            {
                _attackEffects.CancelAttack();
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
