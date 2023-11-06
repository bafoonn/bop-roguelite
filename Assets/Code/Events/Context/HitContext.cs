using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class HitContext : EventContext
    {
        public readonly IHittable Target = null;
        public readonly float Damage;
        public HitContext(IHittable target, float damage) : base(EventActionType.OnHit)
        {
            Target = target;
            Damage = damage;
        }
    }
}
