using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public interface IHittable
    {
        public void Hit(float damage);
    }
}
