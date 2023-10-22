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
        }

        [SerializeField] private bool _canStack = true;
        public bool CanStack => _canStack;

        public string Name;
        public string Description;
        public Sprite Sprite;
        public int Amount; // FOR INVENTORY

        public StatEffect[] Effects;
        public EventActionContainer[] Events;

        private List<EventAction> _addedActions = new List<EventAction>();


        public void Loot()
        {
            foreach (var effect in Effects)
            {
                effect.Apply();
            }

            foreach (var container in Events)
            {
                var newAction = EventActions.Create(container.EventActionPrefab, container.ActionType);
                if (newAction != null)
                {
                    _addedActions.Add(newAction);
                }
            }
        }

        public void Drop()
        {
            foreach (var effect in Effects)
            {
                effect.Unapply();
            }

            while (_addedActions.Count > 0)
            {
                _addedActions[0].Remove();
                _addedActions.RemoveAt(0);
            }
        }
    }
}
