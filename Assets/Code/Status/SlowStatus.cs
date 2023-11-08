using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class SlowStatus : IStatusEffect
    {
        public float SlowPercentage;
        public StatusType Type => StatusType.Slow;

        public SlowStatus(float percentage)
        {
            SlowPercentage = percentage;
        }

        public void Apply(ICharacter character, float duration)
        {
            character.Movement.Slow(SlowPercentage, duration);
        }

        public void Update(float deltaTime)
        {
        }

        public void UnApply(ICharacter character)
        {
        }
    }
}
