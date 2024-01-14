using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public abstract class State : MonoBehaviour
    {
        public abstract State EnterState();
        public abstract State RunCurrentState();
    }
}
