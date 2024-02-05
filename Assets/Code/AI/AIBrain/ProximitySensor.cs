using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class ProximitySensor : MonoBehaviour
    {
        private Health _enemyAI;
        private GameObject player;
        private FixedEnemyAI enemyAI;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            _enemyAI = GetComponent<Health>();
            enemyAI = GetComponent<FixedEnemyAI>();
        }

        private void Update()
        {
            if ((player.transform.position - transform.position).magnitude < 1.5f) // Stops enemies from pushing player
            {
                if (enemyAI.canAttack)
                {
                    _enemyAI.TakeDamage(_enemyAI.CurrentHealth);
                }
            }
        }
    }
}
