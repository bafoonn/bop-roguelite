using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    
    public class DealDamageOnTrigger : MonoBehaviour
    {
        [SerializeField] public float damage;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (collision.TryGetComponent<IHittable>(out var hittable))
                {
                    hittable.Hit(damage);
                }
            }
        }
    }
}
