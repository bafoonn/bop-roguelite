using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class FlatHeal : EventAction
    {
        [SerializeField] float _healAmount = 10f;
        private PlayerHealth _health = null;

        protected override void Init()
        {
            _health = _player.GetComponent<PlayerHealth>();
        }

        protected override void Trigger()
        {
            _health.Heal(_healAmount);
        }
    }
}
