using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public abstract class ItemBase : ScriptableObject, IItem
    {
        public abstract bool CanStack { get; }

        public string Name;
        public Sprite Sprite;
        public int Amount; // FOR INVENTORY

        public abstract void Loot();
        public abstract void Drop();
    }
}
