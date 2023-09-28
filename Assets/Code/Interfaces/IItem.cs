using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public interface IItem
    {
        public bool CanStack { get; }
        public void Loot();
        public void Drop();
    }
}
