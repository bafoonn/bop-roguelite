using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Pasta
{
    public class BurnStatus : IStatusEffect
    {
        public float Damage;
        public float Interval;

        public StatusType Type => StatusType.Burn;

        private IHittable _current = null;
        private float timer = 0;

        public BurnStatus(float damage, float inteval = 0.33f)
        {
            Damage = damage;
            Interval = inteval;
        }

        public void Apply(ICharacter character)
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
            }
        }

        public void UnApply()
        {
            _current = null;
        }
    }
}
