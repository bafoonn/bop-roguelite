using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class ItemAbilities : MonoBehaviour
    {
        private static ItemAbilities _actions = null;
        private Player _player = null;

        public static event Action<EventContext> OnEvent;

        private void Awake()
        {
            _actions = this;
        }

        public void Setup(Player player)
        {
            _player = player;
        }

        public static ItemAbility Create(ItemBase item, ItemAbility actionPrefab, EventActionType type, ScalingValue procChance)
        {
            if (_actions == null)
            {
                return null;
            }

            if (_actions._player == null)
            {
                return null;
            }

            if (actionPrefab == null)
            {
                return null;
            }

            var action = Instantiate(actionPrefab, _actions.transform);
            action.gameObject.name = $"{type} - {actionPrefab.name}";
            action.Setup(item, _actions._player, type, procChance);
            return action;
        }

        public static void InvokeEvent(EventContext context)
        {
            if (OnEvent != null) OnEvent(context);
        }

        public static void InvokeEvent(EventActionType type)
        {
            InvokeEvent(new EventContext(type));
        }
    }
}
