using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public interface IItem
    {
        public bool CanStack { get; }
        public bool Loot();
        public void Drop();
    }
}
