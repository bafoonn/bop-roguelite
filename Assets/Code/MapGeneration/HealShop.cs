using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class HealShop : ShopRefresh
    {
        [SerializeField]
        private int healAmount;
        public override void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<Player>(out var player))
            {
                if (player.TryTakeCurrency(cost))
                {
                    player.PlayerHealth.Heal(healAmount);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
