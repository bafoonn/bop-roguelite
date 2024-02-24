using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PeaMinion : MonoBehaviour
    {
        private Transform _target = null;
        private Pea _peaPrefab = null;
        public float FireRate = 1.0f;
        public float Damage = 10.0f;
        private float _lerp = 3f;
        private EnemySensor _sensor;
        private float Interval => 1f / FireRate;

        private void Awake()
        {
            _target = Player.Current.transform;
        }

        public static PeaMinion Spawn(PeaMinion minionPrefab, Pea peaPrefab, EnemySensor sensor, float damage, float fireRate)
        {
            var newMinion = Instantiate(minionPrefab);
            newMinion._sensor = sensor;
            newMinion._peaPrefab = peaPrefab;
            newMinion.Damage = damage;
            newMinion.FireRate = fireRate;
            newMinion.gameObject.Activate();
            return newMinion;
        }

        private void OnEnable()
        {
            StartCoroutine(ShootLoop());
        }

        private IEnumerator ShootLoop()
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
                else
                {
                    Debug.Log("ENEMY NULL");
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
