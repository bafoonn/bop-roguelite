using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public interface IHittable
    {
        public void Hit(float damage, HitType type = HitType.None, ICharacter source = null);
        public MonoBehaviour Mono { get; }
    }
}
