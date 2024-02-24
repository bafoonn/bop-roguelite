using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EnemySensor : Sensor<IEnemy>
    {
        private void OnValidate()
        {
            SensedLayers = 1 << LayerMask.NameToLayer("Enemy");
        }

        public IEnemy ClosestEnemy()
        {
            IEnemy closest = null;
            for (int i = 0; i < _objects.Count; i++)
            {
                var enemy = _objects[i];
                if (enemy == null) continue;
                if (closest == null)
                {
                    closest = enemy;
                    continue;
                }

                Vector3 closestPos = closest.Mono.transform.position;
                Vector3 enemyPos = enemy.Mono.transform.position;
                float a = Vector2.SqrMagnitude(transform.position - closestPos);
                float b = Vector2.SqrMagnitude(transform.position - enemyPos);

                if (a > b || closest == null)
                {
                    closest = enemy;
                }
            }

            return closest;
        }
    }
}
