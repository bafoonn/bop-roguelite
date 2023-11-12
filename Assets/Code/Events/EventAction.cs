using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pasta
{
    public abstract class EventAction : MonoBehaviour
    {
        protected Player _player = null;
        protected EventActionType _actionType = EventActionType.None;
        protected float _procChance = 1;

        public UnityEvent OnInit;
        public UnityEvent OnTrigger;

        private void OnEnable()
        {
            EventActions.OnEvent += OnEvent;
        }

        private void OnDisable()
        {
            EventActions.OnEvent -= OnEvent;
        }

        private void OnEvent(EventContext context)
        {
            if (context.EventType != _actionType) return;
            if (UnityEngine.Random.value > _procChance) return;

            if (OnTrigger != null)
            {
                OnTrigger.Invoke();
            }

            Trigger(context);
        }

        public virtual void Setup(Player player, EventActionType type, float procChance)
        {
            _player = player;
            _actionType = type;
            _procChance = Mathf.Clamp01(procChance);
            Init();
            if (OnInit != null)
            {
                OnInit.Invoke();
            }
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
