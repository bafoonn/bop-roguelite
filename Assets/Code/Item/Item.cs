using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public abstract class Item : ScriptableObject, IItem
    {
        public string Name;
        public Sprite Sprite;
        public abstract void Drop();

        public abstract void Loot();
    }
}
