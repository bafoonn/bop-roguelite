using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [RequireComponent(typeof(Collider2D))]
    public class AttackArea : Sensor<IHittable>
    {
        /// <summary>
        /// Hits detected objects.
        /// </summary>
        /// <returns>Amount of objects hit.</returns>
        public int HitObjects(float damage, HitType hitType = HitType.None, ICharacter source = null, Action<IHittable> onHit = null)
        {
            int hitCount = 0;
            for (int i = 0; i < Objects.Count; i++)
            {
                var hittable = Objects[i];
                if (hittable == null)
                {
                    continue;
                }

                hittable.Hit(damage, hitType, source);
                if (onHit != null)
                {
                    onHit(hittable);
                }
                hitCount++;
            }
            return hitCount;
        }
    }
}
