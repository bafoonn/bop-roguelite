using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Items/StatItem", fileName = "New StatItem")]
    public class StatItem : ItemBase
    {
        [SerializeField] private bool _canStack = true;
        public StatEffect[] Effects;

        public override bool CanStack => _canStack;

        public override void Drop()
        {
            foreach (var effect in Effects)
            {
                effect.Unapply();
            }
        }

        public override void Loot()
        {
            foreach (var effect in Effects)
            {
                effect.Apply();
            }
        }
    }
}
