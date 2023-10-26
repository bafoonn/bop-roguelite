using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Heal : EventAction
    {
        [SerializeField] private float _healAmount;
        [SerializeField] private bool _isPercentage = false;
        private PlayerHealth _health = null;

        protected override void Init()
        {
            _health = _player.GetComponent<PlayerHealth>();
        }

        protected override void Trigger(EventContext context)
        {
            _health.Heal(_isPercentage ? _health.MaxHealth * _healAmount : _healAmount);
        }
    }
}
