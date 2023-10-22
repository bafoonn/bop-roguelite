using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EnemySensor : Sensor<IHittable>
    {
        protected override void Awake()
        {
            base.Awake();
            SensedLayers = 1 << LayerMask.NameToLayer("Enemy");
        }
    }
}
