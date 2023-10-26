using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class ChainLightning : EventAction
    {
        [SerializeField] private float _procChance = 0.3f;
        [SerializeField] private float _radius = 5f;
        [SerializeField] private int _chains = 3;
        [SerializeField] private float _chainTime = 0.5f;
        [SerializeField] private float _damageCoefficiency = 0.8f;
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
            if (context.GetType() == typeof(HitContext))
            {
                var hitContext = (HitContext)context;
                firstTarget = hitContext.Target;
            }
            if (Random.value < _procChance)
            {
                StartCoroutine(SpawnLightning(firstTarget));
            }
        }

        private IEnumerator SpawnLightning(IHittable first)
        {
            int chainCount = 0;
            List<IHittable> hits = new List<IHittable>();
            Vector2 point = first != null ? first.Mono.transform.position : _player.transform.position;
            while (chainCount < _chains)
            {
                var enemies = new List<Collider2D>(Physics2D.OverlapCircleAll(point, _radius, _enemyLayer));
                bool targetFound = false;

                if (chainCount == 0 && first != null)
                {
                    hits.Add(first);
                    first.Hit(_damage.Value * _damageCoefficiency);
                }

                while (enemies.Count > 0 && !targetFound)
                {
                    int random = Random.Range(0, enemies.Count);
                    if (enemies[random].TryGetComponent<IHittable>(out var hittable))
                    {
                        if (hits.Contains(hittable))
                        {
                            enemies.RemoveAt(random);
                            continue;
                        }

                        point = enemies[random].transform.position;

                        if (_particles != null)
                        {
                            _particles.transform.position = point;
                            _particles.Play();
                        }
                        hits.Add(hittable);
                        hittable.Hit(_damage.Value * _damageCoefficiency);
                        targetFound = true;
                    }
                    enemies.RemoveAt(random);
                }

                chainCount++;
                yield return new WaitForSeconds(_chainTime);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
