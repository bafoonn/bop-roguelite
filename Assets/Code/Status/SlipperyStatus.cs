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
        private List<ICharacter> _applied = new List<ICharacter>();

        public void Apply(ICharacter character, float duration)
        {
            if (_applied.Contains(character)) return;
            _applied.Add(character);
            character.Movement.MakeSlippery();
            _affectedMovement = character.Movement;
        }

        public void UnApply(ICharacter character)
        {
            if (!_applied.Contains(character)) return;
            _affectedMovement.Unslip();
            _applied.Remove(character);
        }

        public void Update(ICharacter character, float deltaTime)
        {
        }

        public int Compare(IStatusEffect other)
        {
            return 0;
        }
    }
}
