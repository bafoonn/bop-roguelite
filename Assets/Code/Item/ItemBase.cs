using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Scriptables/Item", fileName = "New Item")]
    public class ItemBase : ScriptableObject, IItem
    {
        [System.Serializable]
        public class ItemAbilityContainer
        {
            public string Name;
            public ItemAbility ItemAbilityPrefab;
            public EventActionType ActionType;
            public ScalingValue ProcChance;
        }

        [SerializeField] private bool _canStack = true;
        private int _amount = 0;
        public bool CanStack => _canStack;
        public bool IsLooted => _amount > 0;
        public bool CanLoot
        {
            get
            {
                if (!CanStack && IsLooted) return false;
                return true;
            }
        }

        public string Name;
        [TextArea(2, 4)]
        public string Description;
        [TextArea(1, 2)]
        public string Flavor;
        public Sprite Sprite;
        public int Amount => _amount;
        public int cost;
        public StatEffect[] Effects;
        public ItemAbilityContainer[] Abilities;
        public Hieroglyph[] Hieroglyphs;

        private Stack<List<StatEffect>> _addedEffects = new Stack<List<StatEffect>>();
        private List<ItemAbility> _addedAbilities = new List<ItemAbility>();

        public event System.Action OnAmountChanged;

        public bool Loot()
        {
            if (!CanLoot)
            {
                return false;
            }

            if (Amount == 0 && Abilities != null)
            {
                foreach (var container in Abilities)
                {
                    var newAction = ItemAbilities.Create(this, container.ItemAbilityPrefab, container.ActionType, container.ProcChance);
                    if (newAction != null)
                    {
                        _addedAbilities.Add(newAction);
                    }
                }
            }

            var effects = new List<StatEffect>();
            foreach (var effect in Effects)
            {
                effect.Apply();
                effects.Add(effect);
            }
            _addedEffects.Push(effects);

            _amount++;
            if (OnAmountChanged != null) OnAmountChanged.Invoke();
            return true;
        }

        public void Drop()
        {
            if (Amount > 0) _amount--;

            var effects = _addedEffects.Pop();
            foreach (var effect in effects)
            {
                effect.Unapply();
            }

            if (Amount == 0)
            {
                while (_addedAbilities.Count > 0)
                {
                    if (_addedAbilities[0] != null) _addedAbilities[0].Remove();
                    _addedAbilities.RemoveAt(0);
                }
            }
            if (OnAmountChanged != null) OnAmountChanged.Invoke();
        }
    }
}
