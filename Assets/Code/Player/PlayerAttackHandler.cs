using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PlayerAttackHandler : MonoBehaviour
    {
        public float QuickAttackTime = 0.2f;
        public float HeavyAttackTime = 0.8f;

        private Coroutine _attackRoutine;

        public bool CanAttack => _attackRoutine == null;
        public bool IsAttacking => _attackRoutine != null;
        private AttackArea _sensor;

        private bool _cancellable = true;

        private void Awake()
        {
            _sensor = GetComponentInChildren<AttackArea>();
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
            yield return new WaitForSeconds(QuickAttackTime);
            foreach (var hittable in _sensor.Objects)
            {
                hittable.Hit(100);
            }
            _attackRoutine = null;
        }

        private IEnumerator HeavyAttack(Vector2 dir)
        {
            float waitTime = HeavyAttackTime * 0.5f;
            yield return new WaitForSeconds(waitTime);

            _cancellable = false;
            yield return new WaitForSeconds(waitTime);

            foreach (var hittable in _sensor.Objects)
            {
                hittable.Hit(30);
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
    }
}
