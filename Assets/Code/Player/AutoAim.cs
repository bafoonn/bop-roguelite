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
            Vector2 closest = Vector2.zero;
            foreach (var enemy in _enemySensor.Objects)
            {
                if (closest == Vector2.zero)
                {
                    closest = enemy.Mono.transform.position;
                    continue;
                }

                float a = Vector2.SqrMagnitude((Vector2)transform.position - closest);
                float b = Vector2.SqrMagnitude(transform.position - enemy.Mono.transform.position);

                if (a > b)
                {
                    closest = enemy.Mono.transform.position;
                }
            }

            return (closest - (Vector2)transform.position);
        }
    }
}
