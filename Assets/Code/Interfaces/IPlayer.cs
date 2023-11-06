using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public interface IPlayer : ICharacter
    {
        public Loot Loot { get; }
        public InputReader Input { get; }
        public PlayerHealth PlayerHealth { get; }
    }
}
