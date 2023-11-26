using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Pasta
{
    /// <summary>
    /// Does damage in intervals. Won't work properly if the same instance is applied to multiple StatusHandlers.
    /// </summary>
    public class BurnStatus : IStatusEffect
    {
        public float Damage;
        public readonly float Interval = 1f;

        public StatusType Type => StatusType.Burn;

        public bool CanStack => false;

        private IHittable _current = null;
        private float timer = 0;

        public BurnStatus(float damage)
        {
            Damage = damage;
        }

        public BurnStatus()
        {
        }

        public void Apply(ICharacter character, float duration)
        {
            _current = character;
        }

        public void Update(float deltaTime)
        {
            if (_current == null) return;

            timer += deltaTime;
            if (timer > Interval)
            {
                _current.Hit(Damage);
                timer = 0;
            }
        }

        public void UnApply(ICharacter character)
        {
            _current = null;
            timer = 0;
        }

        public int Compare(IStatusEffect other)
        {
            if (other is BurnStatus burnStatus)
            {
                if (burnStatus.Damage > Damage) return -1;
                if (burnStatus.Damage < Damage) return 1;
            }

            return 0;
        }
    }
}
