using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class AutoAim : MonoBehaviour
    {
        private EnemySensor _enemySensor = null;
        public bool EnemiesInRange => _enemySensor.Objects.Count > 0;

        private void Awake()
        {
            _enemySensor = this.AddOrGetComponent<EnemySensor>();
        }

        public Vector2 ClosestEnemyDir()
        {
            var enemy = _enemySensor.ClosestEnemy();
            if (enemy == null) return Vector2.zero;
            return ((Vector2)enemy.Mono.transform.position - (Vector2)transform.position).normalized;
        }
    }
}
