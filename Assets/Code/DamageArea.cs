using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
                for (int i = 0; i < _objects.Count; i++)
                {
                    var hittable = _objects[i];
                    Debug.Log(hittable);
                    if (hittable == null)
                    {
                        continue;
                    }

                    hittable.Hit(Damage);
                }
            }
        }
    }
}
