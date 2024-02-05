using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PoisonStatus : IStatusEffect
    {
        public float Damage;
        public float Interval => 1f;

        public StatusType Type => StatusType.Poison;
        public bool CanStack => true;

        public PoisonStatus(float damage)
        {
            Damage = damage;
        }

        public void Apply(ICharacter character, float duration)
        {
        }

        public void Update(ICharacter character, float deltaTime)
        {
            if (character == null) return;
            character.Hit(Damage, HitType.Status);
        }

        public void UnApply(ICharacter character)
        {
        }

        public int Compare(IStatusEffect other)
        {
            if (other is PoisonStatus poison)
            {
                if (poison.Damage > Damage) return -1;
                if (poison.Damage < Damage) return 1;
            }
            return 0;
        }
    }
}
