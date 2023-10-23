using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pasta
{
    public interface IPickup
    {
        public UnityEvent OnPickup { get; }
        public void Take();
    }
}
