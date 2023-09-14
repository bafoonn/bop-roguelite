using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class AttackArea : Sensor<IHittable>
    {
    }
}
