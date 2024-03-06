using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PeaShooter : ItemAbility
    {
        [SerializeField] private Pea _peaPrefab = null;
        [SerializeField] private PeaMinion _minionPrefab = null;
        [SerializeField] private float _fireRate = 1.0f;
        [SerializeField] private float _damage = 10.0f;
        [SerializeField] private float _damagePerStack = 1.0f;
        [SerializeField] private float _lerp = 5f;

        private EnemySensor _sensor;

        private Stack<PeaMinion> _minions = new();

        protected override void Init()
        {
            _sensor = GetComponent<EnemySensor>();
            AddMinion();
            _item.OnAmountChanged += OnItemAmountChanged;
        }

        private void OnDestroy()
        {
            _item.OnAmountChanged -= OnItemAmountChanged;
        }

        private void OnItemAmountChanged()
        {
            foreach (var minion in _minions)
            {
                minion.Damage = _damage + _damagePerStack * (_item.Amount - 1);
            }
        }

        private void AddMinion()
        {
            var minion = PeaMinion.Spawn(_minionPrefab, _peaPrefab, _sensor, _damage, _fireRate);
            _minions.Push(minion);
        }

        private void RemoveMinion()
        {
            if (_minions.Count == 1) return;
            var minion = _minions.Pop();
            Destroy(minion.gameObject);
        }
    }
}
