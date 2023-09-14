using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class DamageArea : Sensor<IHittable>
    {
        public float Interval = 0.5f;
        public float Damage = 1f;

        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(Interval);
                foreach (IHittable hittable in _objects)
                {
                    hittable.Hit(Damage);
                }
            }
        }
    }
}
