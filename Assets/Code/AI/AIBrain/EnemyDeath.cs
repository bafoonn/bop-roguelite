using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EnemyDeath : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }


        


       

        public void DeathAnimDone()
        {
            Destroy(gameObject);
        }
    }
}
