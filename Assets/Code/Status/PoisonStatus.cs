using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PoisonStatus : IStatusEffect
    {
        public float Damage;
        public float Interval => 1f;

        private IHittable _current = null;
        private float _timer = 0;

        public StatusType Type => StatusType.Poison;
        public bool CanStack => true;

        public PoisonStatus(float damage)
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
            _current.Hit(Damage, HitType.Status);
        }

        public void UnApply(ICharacter character)
        {
            _current = null;
            _timer = 0;
        }

        public int Compare(IStatusEffect other)
        {
            if (other is PoisonStatus poison)
            {
                if (poison.Damage > Damage) return -1;
                if (poison.Damage < Damage) return 1;
            }
            return 0;
        }
    }
}
