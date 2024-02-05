using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EnemyDeath : MonoBehaviour
    {
        [Header("Explosion that Suicide enemies does.")]
        [SerializeField] private GameObject BulletSpawner;
        // Start is called before the first frame update
        void Start()
        {
            
        }


        

        public void SpawnExplosion()
        {
            Instantiate(BulletSpawner, transform.position, transform.rotation);
        }
       

        public void DeathAnimDone()
        {
            Destroy(gameObject);
        }
    }
}
