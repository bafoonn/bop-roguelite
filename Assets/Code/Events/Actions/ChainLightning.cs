using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class ChainLightning : EventAction
    {
        [SerializeField] private float _procChance = 0.3f;
        [SerializeField] private float _radius = 5f;
        [SerializeField] private int _chainCount = 3;
        [SerializeField] private float _chainTime = 0.5f;
        [SerializeField] private float _damageCoefficiency = 0.8f;
        private Stat _damage = null;
        private int _enemyLayer;

        protected override void Init()
        {
            base.Init();
            _enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
            _damage = StatManager.Current.GetStat(StatType.Damage);
        }

        protected override void Trigger()
        {
            base.Trigger();
            if (Random.value < _procChance)
            {
                StartCoroutine(SpawnLightning());
            }
        }

        private IEnumerator SpawnLightning()
        {
            int chains = 0;
            List<IHittable> hits = new List<IHittable>();
            Vector2 point = transform.position;
            while (chains < _chainCount)
            {
                var enemyColliders = Physics2D.OverlapCircleAll(point, _radius, _enemyLayer);
                for (int i = 0; i < enemyColliders.Length; i++)
                {
                    if (enemyColliders[i].TryGetComponent<IHittable>(out var hittable))
                    {
                        if (hits.Contains(hittable)) continue;

                        point = enemyColliders[i].transform.position;
                        hits.Add(hittable);
                        hittable.Hit(_damage.Value * _damageCoefficiency);
                    }
                }

                chains++;
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
