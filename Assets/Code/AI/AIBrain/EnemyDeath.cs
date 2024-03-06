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

        private void OnEnable()
        {
            ItemAbilities.OnEvent += OnEvent;
        }

        private void OnDisable()
        {
            ItemAbilities.OnEvent -= OnEvent;
        }

        private void OnEvent(EventContext obj)
        {
            if (obj.EventType != EventActionType.OnRoomEnter) return;
            Destroy(this.gameObject);
        }



        public void SpawnExplosion()
        {
            Instantiate(BulletSpawner, transform.position, transform.rotation);
        }
       

        
    }
}
