using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class CoinReward : Coin
    {
        private Level level;
        // Start is called before the first frame update
        void Start()
        {
            level = GetComponentInParent<Level>();
        }
        public override void Take()
        {
            base.Take();
            if (level != null)
            {
                level.PickedUpReward();
            }
        }
    }
}
