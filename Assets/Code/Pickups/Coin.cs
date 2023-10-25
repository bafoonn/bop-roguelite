using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Coin : PickupBase
    {
        public int Value = 1;

        public void Take(ref int currency)
        {
            currency += Value;
            Take();
        }
    }
}
