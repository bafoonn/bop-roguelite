using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Pasta
{
    public class Puddle : MonoBehaviour
    {
        public float damage;
        // Start is called before the first frame update
        void Start()
        {
        }



      

        // Update is called once per frame
        void Update()
        {
            DamageArea[] area = gameObject.GetComponentsInChildren<DamageArea>();
            foreach (DamageArea damagearea in area)
            {
                
                damagearea.Damage = damage;
                

            }
        }
    }
}
