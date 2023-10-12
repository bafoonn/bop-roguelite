using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Scriptables/Item", fileName = "New Item")]
    public class ItemBase : ScriptableObject, IItem
    {
        [SerializeField] private bool _canStack = true;
        public bool CanStack => _canStack;

        public string Name;
        public Sprite Sprite;
        public int Amount; // FOR INVENTORY

        public StatEffect[] Effects;

        public void Loot()
        {
            foreach (var effect in Effects)
            {
                effect.Apply();
            }
        }

        public void Drop()
        {
            foreach (var effect in Effects)
            {
                effect.Unapply();
            }
        }
    }
}
