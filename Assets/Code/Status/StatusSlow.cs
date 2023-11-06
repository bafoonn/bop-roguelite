using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class StatusSlow : IStatusEffect
    {
        public float SlowPercentage;
        public float Duration;
        public StatusType Type => StatusType.Slow;

        public StatusSlow(float percentage, float duration)
        {
            SlowPercentage = percentage;
            Duration = duration;
        }

        public void Apply(ICharacter character)
        {
            character.Movement.Slow(SlowPercentage, Duration);
        }

        public void Update(float deltaTime)
        {
        }

        public void UnApply()
        {
        }
    }
}
