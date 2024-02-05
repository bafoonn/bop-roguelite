using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject Bullet;
        private DamageArea damageArea;
        private float[] rotations;
        [SerializeField] private float howmanyDirections;
        // Start is called before the first frame update
        void Start()
        {
            damageArea = GetComponentInParent<DamageArea>();
            float angle = 360 / howmanyDirections;

            StartCoroutine(Deactivate());
            if(damageArea != null )
            {
                if (damageArea.enabled)
                {
                    for(int i = 0; i < howmanyDirections; i++)
                    {
                        
                        Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, angle * i));
                        
                    }
                    //Instantiate(Bullet, transform.position, Quaternion.Euler(0,0,));
                }
            }
        }



        private IEnumerator Deactivate()
        {
            yield return new  WaitForSeconds(2f);
            Destroy(this.transform.parent.gameObject);
        }






    }
}
