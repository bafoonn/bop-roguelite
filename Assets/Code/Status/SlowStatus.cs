using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class SlowStatus : IStatusEffect
    {
        public float SlowPercentage;
        public StatusType Type => StatusType.Slow;
        public bool CanStack => true;

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

        public int Compare(IStatusEffect other)
        {
            if (other is SlowStatus slowStatus)
            {
                if (slowStatus.SlowPercentage > SlowPercentage) return -1;
                if (slowStatus.SlowPercentage < SlowPercentage) return 1;
            }
            return 0;
        }
    }
}
