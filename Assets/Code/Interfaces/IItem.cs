using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public interface IItem
    {
        public void Loot();
        public void Drop();
    }
}
