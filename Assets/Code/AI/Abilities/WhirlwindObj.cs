using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class WhirlwindObj : MonoBehaviour
    {
        private CircleCollider2D cc2d;
        private Whirlwind whirlWind;
        private IndicatorForAbility indicator;
        [SerializeField] public float damage;
        public float activetime;
        public float radius;
        // Start is called before the first frame update
        void Start()
        {
            indicator = GetComponent<IndicatorForAbility>();
            whirlWind = FindFirstObjectByType<Whirlwind>();
            cc2d = gameObject.AddComponent<CircleCollider2D>();
            cc2d.enabled = false;
            
            StartCoroutine("DoAbility");
            StartCoroutine("Desstroy");
        }



        IEnumerator DoAbility()
        {
            yield return new WaitForSeconds(indicator.IndicatorAliveTimer);
            cc2d.enabled=true;
            cc2d.isTrigger = true;
            cc2d.radius = radius;
            LayerMask mask = LayerMask.GetMask("Player");
            Collider2D[] collider = Physics2D.OverlapCircleAll(gameObject.transform.position, cc2d.radius, mask);

            Debug.Log(collider.Length);
            foreach (Collider2D collider2D in collider)
            {
                if (collider2D.TryGetComponent(out IHittable hittable))
                {
                    
                        hittable.Hit(damage);// DO DAMAGE!
                    
                    
                }
            }
        }

        IEnumerator Desstroy()
        {
            yield return new WaitForSeconds(activetime + 1);
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
