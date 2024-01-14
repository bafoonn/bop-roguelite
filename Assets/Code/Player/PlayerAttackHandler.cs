using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Pasta
{
    public class PlayerAttackHandler : MonoBehaviour
    {
        [System.Serializable]
        public class AttackProfile
        {
            [HideInInspector] public float Damage;
            [HideInInspector] public float TotalAttackTime;
            [HideInInspector] public float ConeAngle;
            public float WindUpPercent;
            public float WindDownPercent;
            public float HitStopTime;
            public float Windup => TotalAttackTime * WindUpPercent;
            public float Winddown => TotalAttackTime * WindDownPercent;
            public float AttackTime => TotalAttackTime - (Windup + Winddown);
            public AttackArea AttackArea;
            public Action PlayEffect;
            public Action OnHit;
        }

        [SerializeField] private float _heavyAttackTimeMultiplier = 3.5f;
        //[SerializeField] private float _quickAttackTime = 0.2f;
        //[SerializeField] private float _quickAttackWindupPercent = 0.2f;
        //[SerializeField] private float _quickAttackWinddownPercent = 0f;

        //[SerializeField] private float _heavyAttackTime = 0.8f;
        //[SerializeField] private float _heavyAttackWindupPercent = 0.5f;
        //[SerializeField] private float _heavyAttackWinddownPercent = 0f;

        [SerializeField] private AttackProfile _quickAttackProfile;
        [SerializeField] private AttackProfile _heavyAttackProfile;
        private Coroutine _attackRoutine;

        public bool CanAttack => _attackRoutine == null;
        public bool IsAttacking => _attackRoutine != null;
        public AttackType CurrentAttack;
        //[SerializeField] private AttackArea _quickAttackArea;
        //[SerializeField] private AttackArea _heavyAttackArea;

        private bool _cancellable = true;

        //private float _quickAttackDamage = 10f;
        //private float _heavyAttackDamage = 20f;
        private Stat _damage;
        private Stat _attackSpeed;

        private AttackEffects _attackEffects;
        private CleavingWeaponAnimations _weaponAnimations;

        //[SerializeField] private float _quickAttackHitStop = 0.03f;
        //[SerializeField] private float _heavyAttackHitStop = 0.06f;

        private ICharacter _player = null;

        public UnityEvent OnQuickHit;
        public UnityEvent OnHeavyHit;
        public UnityEvent OnSwing;

        private void Awake()
        {
            Assert.IsNotNull(_quickAttackProfile.AttackArea, "QuickAttackArea is not set in PlayerAttackHandler.");
            Assert.IsNotNull(_heavyAttackProfile.AttackArea, "HeavyAttackArea is not set in PlayerAttackHandler.");
            _quickAttackProfile.ConeAngle = _quickAttackProfile.AttackArea.GetComponent<Cone>().Angle;
            _heavyAttackProfile.ConeAngle = _heavyAttackProfile.AttackArea.GetComponent<Cone>().Angle;
            _attackEffects = GetComponentInChildren<AttackEffects>();
            _weaponAnimations = GetComponentInParent<CleavingWeaponAnimations>();
            _quickAttackProfile.OnHit = () =>
            {
                HitStopper.Stop(_quickAttackProfile.HitStopTime);
                OnQuickHit?.Invoke();
            };
            _quickAttackProfile.PlayEffect = _attackEffects.QuickAttack;

            _heavyAttackProfile.OnHit = () =>
            {
                HitStopper.Stop(_heavyAttackProfile.HitStopTime);
                OnHeavyHit?.Invoke();
            };
            _heavyAttackProfile.PlayEffect = _attackEffects.HeavyAttack;
        }

        private void Start()
        {
            _damage = StatManager.Current.GetStat(StatType.Damage);
            _damage.ValueChanged += OnDamageChanged;

            _attackSpeed = StatManager.Current.GetStat(StatType.AttackSpeed);
            _attackSpeed.ValueChanged += OnAttackSpeedChanged;
            SetDamage(_damage.Value, _attackSpeed.Value);

            //_hasAttackEffects = _attackEffects != null;
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
            float quickAttackTime = 1f / attackSpeed;
            float heavyAttackTime = quickAttackTime * _heavyAttackTimeMultiplier;

            _quickAttackProfile.TotalAttackTime = quickAttackTime;
            _quickAttackProfile.Damage = damage;

            _heavyAttackProfile.TotalAttackTime = heavyAttackTime;
            _heavyAttackProfile.Damage = damage * (heavyAttackTime / quickAttackTime) + 1;
            _attackEffects.SetIndicatorLifetime(_heavyAttackProfile.AttackTime);
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
                    _attackRoutine = StartCoroutine(AttackRoutine(type, _quickAttackProfile));
                    break;

                case AttackType.Heavy:
                    _attackRoutine = StartCoroutine(AttackRoutine(type, _heavyAttackProfile));
                    break;
            }
            CurrentAttack = type;

            return true;
        }

        public void Attack(AttackType type = AttackType.Quick)
        {
            TryToAttack(type);
        }

        private IEnumerator AttackRoutine(AttackType type, AttackProfile attackProfile)
        {
            _weaponAnimations.Swing(attackProfile.Windup, attackProfile.Winddown, attackProfile.AttackTime, _attackEffects.IsFlipped, attackProfile.ConeAngle);
            yield return new WaitForSeconds(attackProfile.Windup);

            _cancellable = false;
            attackProfile.PlayEffect();
            int hitCount = HitObjects(type, attackProfile.AttackArea, attackProfile.Damage);
            OnSwing.Invoke();
            if (hitCount > 0)
            {
                attackProfile.OnHit();
            }
            //_weaponAnimations.SwingEnd(attackProfile.AttackTime + attackProfile.Winddown, !_attackEffects.IsFlipped, attackProfile.ConeAngle);
            yield return new WaitForSeconds(attackProfile.AttackTime);

            yield return new WaitForSeconds(attackProfile.Winddown);
            _cancellable = true;
            _attackRoutine = null;
        }

        //private IEnumerator QuickAttack()
        //{
        //    if (_hasAttackEffects) _attackEffects.QuickAttack();
        //    int hitCount = HitObjects(AttackType.Quick);
        //    if (hitCount == 0 && OnSwing != null) OnSwing.Invoke();
        //    if (hitCount > 0)
        //    {
        //        if (OnQuickHit != null) OnQuickHit.Invoke();
        //        HitStopper.Stop(_quickAttackHitStop);
        //    }
        //    yield return new WaitForSeconds(_quickAttackTime);
        //    _attackRoutine = null;
        //}

        //private IEnumerator HeavyAttack()
        //{
        //    float waitTime = _heavyAttackTime * 0.5f;
        //    if (_hasAttackEffects) _attackEffects.AttackIndicator();
        //    yield return new WaitForSeconds(waitTime);

        //    _cancellable = false;
        //    yield return new WaitForSeconds(waitTime);
        //    float hitCount = HitObjects(AttackType.Heavy);

        //    if (_hasAttackEffects) _attackEffects.HeavyAttack();
        //    if (hitCount == 0 && OnSwing != null) OnSwing.Invoke();

        //    if (hitCount > 0)
        //    {
        //        if (OnHeavyHit != null) OnHeavyHit.Invoke();
        //        HitStopper.Stop(_heavyAttackHitStop);
        //    }

        //    _cancellable = true;
        //    _attackRoutine = null;
        //}

        private int HitObjects(AttackType type, AttackArea attackArea, float damage)
        {
            int hitCount = attackArea.HitObjects(damage, HitType.Hit, _player, OnHit);

            return hitCount;

            void OnHit(IHittable hittable)
            {
                switch (type)
                {
                    case AttackType.Quick:
                        ItemAbilities.InvokeEvent(new HitContext(hittable, damage, EventActionType.OnQuickHit));
                        break;
                    case AttackType.Heavy:
                        ItemAbilities.InvokeEvent(new HitContext(hittable, damage, EventActionType.OnHeavyHit));
                        break;
                }

                ItemAbilities.InvokeEvent(new HitContext(hittable, damage, EventActionType.OnHit));
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
                _weaponAnimations.StopSwing();
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
