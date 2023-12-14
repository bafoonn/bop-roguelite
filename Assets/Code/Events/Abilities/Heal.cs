using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Heal : ItemAbility
    {
        [SerializeField] private float _healAmount;
        [SerializeField] private bool _isPercentage = false;
        [SerializeField] private bool _scaleWithAmount = true;
        private PlayerHealth _health = null;

        protected override void Init()
        {
            _health = _player.GetComponent<PlayerHealth>();
        }

        protected override void Trigger(EventContext context)
        {
            float value = _isPercentage ? _health.MaxHealth * _healAmount : _healAmount;
            _health.Heal(_scaleWithAmount ? value * _item.Amount : value);
        }

        private void OnValidate()
        {
            if (_isPercentage)
            {
                _healAmount = Mathf.Clamp01(_healAmount);
            }
        }
    }
}
