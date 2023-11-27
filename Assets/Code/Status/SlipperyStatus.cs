using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class SlipperyStatus : IStatusEffect
    {
        public StatusType Type => StatusType.Slippery;
        public bool CanStack => true;

        public float Interval => 0;

        private Movement _affectedMovement = null;

        public void Apply(ICharacter character, float duration)
        {
            character.Movement.MakeSlippery();
            _affectedMovement = character.Movement;
        }

        public void UnApply(ICharacter character)
        {
            _affectedMovement.Unslip();
        }

        public void Update(float deltaTime)
        {
        }

        public int Compare(IStatusEffect other)
        {
            return 0;
        }
    }
}
