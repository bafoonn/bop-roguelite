using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public interface ICharacter : IHittable
    {
        public Health Health { get; }
        public Movement Movement { get; }
        public StatusHandler Status { get; }
    }
}
