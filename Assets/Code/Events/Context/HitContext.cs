using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class HitContext : EventContext
    {
        public IHittable Target = null;
        public HitContext(IHittable target) : base(EventActionType.OnHit)
        {
            Target = target;
        }
    }
}
