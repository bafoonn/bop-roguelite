using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EnemyDeath : MonoBehaviour
    {
        private Drop drop;
        
        // Start is called before the first frame update
        void Start()
        {
            drop = GetComponent<Drop>();
        }


        


       

        public void DeathAnimDone()
        {
            Destroy(gameObject);
        }
    }
}
