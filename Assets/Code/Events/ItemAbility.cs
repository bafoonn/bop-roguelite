using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pasta
{
    public abstract class ItemAbility : MonoBehaviour
    {
        protected Player _player = null;
        protected ItemBase _sourceItem = null;
        protected EventActionType _actionType = EventActionType.None;
        protected ScalingValue _procChance = null;

        public UnityEvent OnInit;
        public UnityEvent OnTrigger;


        private void OnEnable()
        {
            ItemAbilities.OnEvent += OnEvent;
        }

        private void OnDisable()
        {
            ItemAbilities.OnEvent -= OnEvent;
        }

        private void OnEvent(EventContext context)
        {
            if (context.EventType != _actionType) return;
            _procChance.Stacks = _sourceItem.Amount;
            if (!_procChance.Roll()) return;

            if (OnTrigger != null)
            {
                OnTrigger.Invoke();
            }

            Trigger(context);
        }

        public virtual void Setup(ItemBase item, Player player, EventActionType type, ScalingValue procChance)
        {
            _sourceItem = item;
            _player = player;
            _actionType = type;
            _procChance = procChance;
            Init();
            OnInit.Invoke();
        }

        public void Remove()
        {
            Destroy(gameObject);
        }

        protected virtual void Init()
        {
        }

        protected virtual void Trigger(EventContext context)
        {
        }
    }
}
