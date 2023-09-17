using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Items/StatItem", fileName = "New StatItem")]
    public class StatItem : Item
    {
        public StatEffect[] Effects;

        public override void Drop()
        {
            foreach (var effect in Effects)
            {
                StatManager.Current.RemoveEffectFromStat(effect);
            }
        }

        public override void Loot()
        {
            foreach (var effect in Effects)
            {
                StatManager.Current.AddEffectToStat(effect);
            }
        }
    }
}
