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

        private void OnEvent(EventActionType type)
        {
            if (type == _actionType)
            {
                if (OnTrigger != null)
                {
                    OnTrigger.Invoke();
                }

                Trigger();
            }
        }

        public virtual void Setup(Player player, EventActionType type)
        {
            _player = player;
            _actionType = type;
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

        protected virtual void Trigger()
        {
        }
    }
}
