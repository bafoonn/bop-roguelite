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

        private Coroutine _heavyRoutine;
        private Coroutine _quickRoutine;

        public bool CanQuickAttack => _quickRoutine == null;
        public bool CanHeavyAttack => _heavyRoutine == null;
        private HittableSensor _sensor;

        private void Awake()
        {
            _sensor = GetComponentInChildren<HittableSensor>();
        }

        public enum AttackType
        {
            Quick,
            Heavy
        }

        public bool TryToAttack(Vector2 dir, AttackType type = AttackType.Quick)
        {
            switch (type)
            {
                case AttackType.Quick:
                    if (!CanQuickAttack)
                    {
                        return false;
                    }

                    _quickRoutine = StartCoroutine(QuickAttack(dir));

                    break;
                case AttackType.Heavy:
                    return false;
            }
            return true;
        }

        private IEnumerator QuickAttack(Vector2 dir)
        {
            yield return new WaitForSeconds(QuickAttackTime);
            foreach (var hittable in _sensor.Items)
            {
                hittable.Hit(10);
            }
            _quickRoutine = null;
        }

        private IEnumerator HeavyAttack(Vector2 dir)
        {
            throw new NotImplementedException();
        }
    }
}
