using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PlayerCloseSensor : MonoBehaviour
    {
        public float radius = 10;
        
        public float precentage = 30;
        private float precentageSubtract;
        private float result;
        private EnemyAi enemyai;
        // Start is called before the first frame update
        void Start()
        {
            precentageSubtract = precentage / 100;
            
        }

        // Update is called once per frame
        void Update()
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radius);
            result = hitColliders.Length - (hitColliders.Length * precentageSubtract);
            for(int i = 0; i < hitColliders.Length; i++)
			{
                if(i < result) // Check if the current hitcollider is inside the result
                {
                    if(hitColliders[i].gameObject.GetComponent<EnemyAi>() != null)
					{
                        enemyai = hitColliders[i].gameObject.GetComponent<EnemyAi>();
                    }
				}
			}
        }
    }
}
