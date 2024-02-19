using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PeaShooter : ItemAbility
    {
        private Transform _target = null;
        [SerializeField] private Pea _peaPrefab = null;
        [SerializeField] private float FireRate = 1.0f;
        [SerializeField] private float Damage = 10.0f;
        [SerializeField] private float _lerp = 5f;
        private EnemySensor _sensor;
        private float Interval => 1f / FireRate;

        protected override void Init()
        {
            _target = _player.transform;
            _sensor = GetComponent<EnemySensor>();
        }

        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(Interval);
                var enemy = _sensor.ClosestEnemy();
                if (enemy != null)
                {
                    var pea = Pea.Spawn(_peaPrefab, Damage, transform.position);
                    var direction = enemy.Mono.transform.position - transform.position;
                    direction.Normalize();
                    pea.Shoot(direction, 6f);
                }
            }
        }

        private void Update()
        {
            Vector2 targetPosition = _target.position;
            targetPosition += -((Vector2)_target.position - (Vector2)transform.position).normalized;
            transform.position = Vector2.Lerp(transform.position, targetPosition, _lerp * Time.deltaTime);
        }
    }
}
