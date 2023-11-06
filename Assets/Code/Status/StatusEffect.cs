using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public interface IStatusEffect
    {
        public void Apply(ICharacter character);
        public void Update(float deltaTime);
        public void UnApply();
        public StatusType Type { get; }
    }
}
