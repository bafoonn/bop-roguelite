using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PlayerHealthBar : HealthBar
    {
        private void Start()
        {
            SetHealth(Player.Current.Health);
        }
    }
}
