using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Puddle : MonoBehaviour
    {
        private WetFloor wetFloor;
        // Start is called before the first frame update
        void Start()
        {
            wetFloor = FindFirstObjectByType<WetFloor>();
        }

        // Update is called once per frame
        void Update()
        {
            DamageArea[] area = gameObject.GetComponentsInChildren<DamageArea>();
            foreach (DamageArea damagearea in area)
            {
                if (wetFloor != null)
                {
                    damagearea.Damage = wetFloor.damage;
                }

            }
        }
    }
}
