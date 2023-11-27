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
        public float Interval => 1f;

        public StatusType Type => StatusType.Burn;

        public bool CanStack => false;

        private IHittable _current = null;

        public BurnStatus(float damage)
        {
            Damage = damage;
        }

        public void Apply(ICharacter character, float duration)
        {
            _current = character;
        }

        public void Update(float deltaTime)
        {
            if (_current == null) return;
            _current.Hit(Damage);
        }

        public void UnApply(ICharacter character)
        {
            _current = null;
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
