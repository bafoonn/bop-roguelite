using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        [SerializeField] private LayerMask layerMask;
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


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (collision.TryGetComponent(out IHittable hittable))
                {
                    hittable.Hit(damage);// DO DAMAGE!


                }
            }
        }

        IEnumerator DoAbility()
        {
            yield return new WaitForSeconds(indicator.IndicatorAliveTimer);
            cc2d.enabled = true;
            cc2d.isTrigger = true;
            cc2d.radius = radius;
            Collider2D[] collider = Physics2D.OverlapCircleAll(gameObject.transform.position, cc2d.radius, layerMask);

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
