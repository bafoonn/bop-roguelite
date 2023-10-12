using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public abstract class EventAction : MonoBehaviour
    {
        protected Player _player = null;
        protected EventActionType _actionType = EventActionType.None;

        protected virtual void Setup(Player player, EventActionType type)
        {
            _player = player;

            _actionType = type;
        }

        public void Remove()
        {
            Destroy(gameObject);
        }

        protected abstract void Trigger(float value, bool isPercentage);

        public static EventAction Create(Transform actions, Player player, Type action, EventActionType type)
        {
            if (!action.IsSubclassOf(typeof(EventAction)))
            {
                return null;
            }

            var obj = new GameObject(action.Name);
            obj.transform.parent = actions;
            var newAction = (EventAction)obj.AddComponent(action);
            newAction.Setup(player, type);
            return newAction;
        }

        public static EventAction Create<T>(Transform actions, Player player, EventActionType type) where T : EventAction
        {
            return Create(actions, player, typeof(T), type);
        }
    }
}
