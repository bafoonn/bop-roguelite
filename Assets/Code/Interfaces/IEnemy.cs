using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public interface IEnemy : ICharacter
    {
        public bool IsBoss { get; }
    }
}
