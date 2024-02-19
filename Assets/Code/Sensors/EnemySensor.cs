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
            var enemies = new List<IEnemy>(_objects);
            var closestPosition = Vector2.zero;
            IEnemy closest = null;
            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                if (enemy == null) continue;

                float a = Vector2.SqrMagnitude((Vector2)transform.position - closestPosition);
                float b = Vector2.SqrMagnitude(transform.position - enemy.Mono.transform.position);

                if (a > b)
                {
                    closest = _objects[i];
                }
            }

            return closest;
        }
    }
}
