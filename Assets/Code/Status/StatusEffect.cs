using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public interface IStatusEffect
    {
        public void Apply(ICharacter character, float duration);
        public void Update(float deltaTime);
        public void UnApply(ICharacter character);
        public StatusType Type { get; }
    }
}
