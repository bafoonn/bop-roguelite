using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class ApplySlippery : EventAction
    {
        private SlipperyStatus status;
        public float Duration = 5f;

        protected override void Trigger(EventContext context)
        {
            if (context.GetType() != typeof(HitContext))
            {
                return;
            }

            var hitContext = (HitContext)context;
            if (hitContext.Target is ICharacter character)
            {
                character.Status.ApplyStatus(status, Duration);
            }
        }
    }
}
