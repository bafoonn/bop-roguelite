using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Pasta
{
    public class PlayerAttackHandler : MonoBehaviour
    {
        [SerializeField] private float _heavyAttackTimeMultiplier = 3.5f;
        [SerializeField]
        private float _quickAttackTime = 0.2f;
        [SerializeField]
        private float _heavyAttackTime = 0.8f;

        private Coroutine _attackRoutine;

        public bool CanAttack => _attackRoutine == null;
        public bool IsAttacking => _attackRoutine != null;
        public AttackType CurrentAttack;
        [SerializeField] private AttackArea _quickAttackArea;
        [SerializeField] private AttackArea _heavyAttackArea;

        private bool _cancellable = true;

        private float _quickAttackDamage = 10f;
        private float _heavyAttackDamage = 20f;
        private Stat _damage;
        private Stat _attackSpeed;

        private AttackEffects _attackEffects;
        private bool _hasAttackEffects = false;

        [SerializeField] private float _quickAttackHitStop = 0.03f;
        [SerializeField] private float _heavyAttackHitStop = 0.06f;

        private ICharacter _player = null;

        public UnityEvent OnQuickHit;
        public UnityEvent OnHeavyHit;
        public UnityEvent OnMiss;

        private void Awake()
        {
            Assert.IsNotNull(_quickAttackArea, "QuickAttackArea is not set in PlayerAttackHandler.");
            Assert.IsNotNull(_heavyAttackArea, "HeavyAttackArea is not set in PlayerAttackHandler.");
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
            _player = GetComponentInParent<Player>();
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

        public bool TryToAttack(AttackType type = AttackType.Quick)
        {
            if (!CanAttack)
            {
                return false;
            }

            switch (type)
            {
                case AttackType.Quick:
                    _attackRoutine = StartCoroutine(QuickAttack());
                    break;

                case AttackType.Heavy:
                    _attackRoutine = StartCoroutine(HeavyAttack());
                    break;
            }
            CurrentAttack = type;

            return true;
        }

        public void Attack(AttackType type = AttackType.Quick)
        {
            TryToAttack(type);
        }

        private IEnumerator QuickAttack()
        {
            if (_hasAttackEffects) _attackEffects.QuickAttack();
            int hitCount = HitObjects(AttackType.Quick);
            if (hitCount == 0 && OnMiss != null) OnMiss.Invoke();
            if (hitCount > 0)
            {
                if (OnQuickHit != null) OnQuickHit.Invoke();
                HitStopper.Stop(_quickAttackHitStop);
            }
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
            float hitCount = HitObjects(AttackType.Heavy);

            if (_hasAttackEffects) _attackEffects.HeavyAttack();
            if (hitCount == 0 && OnMiss != null) OnMiss.Invoke();

            if (hitCount > 0)
            {
                if (OnHeavyHit != null) OnHeavyHit.Invoke();
                HitStopper.Stop(_heavyAttackHitStop);
            }

            _cancellable = true;
            _attackRoutine = null;
        }

        private int HitObjects(AttackType type)
        {
            int hitCount = 0;
            var sensor = type == AttackType.Quick ? _quickAttackArea : _heavyAttackArea;
            for (int i = 0; i < sensor.Objects.Count; i++)
            {
                var hittable = sensor.Objects[i];
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
                hittable.Hit(damage, HitType.Hit, _player);
                hitCount++;
            }
            return hitCount;
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
