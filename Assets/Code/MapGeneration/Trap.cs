using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Trap : MonoBehaviour
    {
        public bool Disabled;
        public virtual void Disable()
        {
            Disabled = true;
        }
    }
}
