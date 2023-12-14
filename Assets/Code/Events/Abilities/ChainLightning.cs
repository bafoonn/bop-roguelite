using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class ChainLightning : ItemAbility
    {
        [SerializeField] private float _radius = 5f;
        [SerializeField] private int _baseChains = 3;
        [SerializeField] private float _chainTime = 0.5f;
        [SerializeField] private float _damageCoefficiency = 0.8f;
        [SerializeField] private int _chainStackIncrease = 1;
        [SerializeField] private ParticleSystem _particles = null;
        private Stat _damage = null;
        private int _enemyLayer;

        protected override void Init()
        {
            base.Init();
            _enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
            _damage = StatManager.Current.GetStat(StatType.Damage);
            _particles = GetComponentInChildren<ParticleSystem>();
        }

        protected override void Trigger(EventContext context)
        {
            base.Trigger(context);
            IHittable firstTarget = null;
            if (context is HitContext hitContext)
            {
                firstTarget = hitContext.Target;
            }
            StartCoroutine(SpawnLightning(firstTarget));
        }

        private IEnumerator SpawnLightning(IHittable first)
        {
            int chainCount = 0;
            List<IHittable> hitEnemies = new List<IHittable>();
            Vector2 point = first != null ? first.Mono.transform.position : _player.transform.position;
            while (chainCount < _baseChains + _chainStackIncrease * _item.Amount)
            {
                var enemies = new List<Collider2D>(Physics2D.OverlapCircleAll(point, _radius, _enemyLayer));
                bool targetFound = false;

                if (chainCount == 0 && first != null)
                {
                    hitEnemies.Add(first);
                    first.Hit(_damage.Value * _damageCoefficiency);
                    PlayParticles();
                    yield return new WaitForSeconds(_chainTime);
                }

                // 
                while (enemies.Count > 0 && !targetFound)
                {
                    var enemy = FindTarget(enemies);
                    if (enemy == null) continue;

                    PlayParticles();
                    hitEnemies.Add(enemy);
                    enemy.Hit(_damage.Value * _damageCoefficiency);
                    targetFound = true;
                }

                if (!targetFound) break;
                chainCount++;
                yield return new WaitForSeconds(_chainTime);
            }

            IHittable FindTarget(List<Collider2D> enemies)
            {
                int random = Random.Range(0, enemies.Count);
                if (enemies[random].TryGetComponent<IHittable>(out var hittable))
                {
                    if (hitEnemies.Contains(hittable))
                    {
                        enemies.RemoveAt(random);
                    }

                    point = enemies[random].transform.position;
                    return hittable;
                }
                enemies.RemoveAt(random);
                return null;
            }

            void PlayParticles()
            {
                if (_particles != null)
                {
                    _particles.transform.position = point;
                    _particles.Play();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
