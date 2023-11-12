using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Scriptables/Item", fileName = "New Item")]
    public class ItemBase : ScriptableObject, IItem
    {
        [System.Serializable]
        public class EventActionContainer
        {
            public string Name;
            public EventAction EventActionPrefab;
            public EventActionType ActionType;
            [Range(0f, 1f)] public float ProcChance = 1;
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
        public string Description;
        public string Flavor;
        public Sprite Sprite;
        public int Amount => _amount;
        public int cost;
        public StatEffect[] Effects;
        public EventActionContainer[] Events;

        private List<EventAction> _addedActions = new List<EventAction>();

        public bool Loot()
        {
            if (!CanLoot)
            {
                return false;
            }

            _amount++;

            foreach (var effect in Effects)
            {
                effect.Apply();
                Debug.Log(effect);
            }

            foreach (var container in Events)
            {
                var newAction = EventActions.Create(container.EventActionPrefab, container.ActionType, container.ProcChance);
                if (newAction != null)
                {
                    _addedActions.Add(newAction);
                }
            }

            return true;
        }

        public void Drop()
        {
            if (Amount > 0)
            {
                _amount--;
            }

            foreach (var effect in Effects)
            {
                effect.Unapply();
            }

            while (_addedActions.Count > 0)
            {
                if (_addedActions[0] != null) _addedActions[0].Remove();
                _addedActions.RemoveAt(0);
            }
        }
    }
}
