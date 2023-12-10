using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class ApplySlow : ItemAbility
    {
        public float SlowPercentage = 0.5f;
        public float Duration = 5f;

        protected override void Trigger(EventContext context)
        {
            base.Trigger(context);
            if (context is HitContext hitContext)
            {
                if (hitContext.Target is ICharacter character)
                {
                    character.Status.ApplyStatus(new SlowStatus(SlowPercentage), Duration);
                }
            }
        }
    }
}
