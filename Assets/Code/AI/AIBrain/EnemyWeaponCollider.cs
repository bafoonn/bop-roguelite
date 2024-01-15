using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EnemyWeaponCollider : MonoBehaviour
    {
        public EnemyAi m_Ai;
        private FixedEnemyAI fixedAI;
        [SerializeField] private bool isNormalEnemy = true;
        public BossAI bossAI;
        private float damage = 2f;
        private WeaponParent weaponParent;
        private void Start()
        {
            if (isNormalEnemy)
            {
                m_Ai = GetComponentInParent<EnemyAi>();
                if(m_Ai != null)
				{
                    damage = m_Ai.damage;
                }
				else
				{
                    fixedAI = GetComponentInParent<FixedEnemyAI>();
                    damage = fixedAI.damage;
				}
               
            }
            else
            {
                bossAI = GetComponentInParent<BossAI>();
                damage = bossAI.damage;
            }
            weaponParent = GetComponentInParent<WeaponParent>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (collision.TryGetComponent<IHittable>(out var hittable))
                {
                    hittable.Hit(damage, source: isNormalEnemy ? m_Ai : bossAI);
                    if (!weaponParent.Aim)
                    {
                        //hittable.Hit(damage);
                    }
                }
            }
        }
    }
}
