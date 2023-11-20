using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PlayerCloseSensor : MonoBehaviour
    {
        public float radius = 20;

        public float precentage = 40;
        private float precentageSubtract;
        public float result;
        private EnemyAi enemyai;
        [SerializeField] private LayerMask layermask;
        private int layer;
        public int maxEnemiesThatcanAttack = 4;
        // Start is called before the first frame update
        void Start()
        {
            precentageSubtract = precentage / 100;
            layer = layermask;

        }

        // Update is called once per frame
        void Update()
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, layermask); // TODO: Only affect the X Closest
            result = Mathf.Max(result, 1); // Ensure result is at least 1 
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (i < maxEnemiesThatcanAttack) // Check if the current hitcollider is inside the result
                {
                    //Debug.Log(i);
                    if (hitColliders[i].gameObject.TryGetComponent(out EnemyAi enemyAi))
                    {
                        enemyai = hitColliders[i].gameObject.GetComponent<EnemyAi>();
                        enemyai.canAttack = true;
                        AIData aidata = hitColliders[i].gameObject.GetComponent<AIData>();
                    }
                }
                else
                {
                    if (hitColliders[i].gameObject.TryGetComponent(out EnemyAi enemyAi))
                    {
                        enemyai = hitColliders[i].gameObject.GetComponent<EnemyAi>();
                        enemyai.canAttack = false;
                        AIData aidata = hitColliders[i].gameObject.GetComponent<AIData>();
                        aidata.currentTarget = null;
                    }
                }

            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position, radius);
        }
    }
}
