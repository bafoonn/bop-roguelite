using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Pasta
{
    public class ShopRefresh : MonoBehaviour
    {
        private Shopkeeper shopKeeper;
        [SerializeField]
        public int cost;
        private string costString;
        private TMP_Text costDisplay;
        // Start is called before the first frame update
        void Start()
        {
            shopKeeper = GetComponentInParent<Shopkeeper>();
            costDisplay = GetComponentInChildren<TMP_Text>();
            costString = cost.ToString();
            costDisplay.text = costString;
        }

        public virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<Player>(out var player))
            {
                if (player.TryTakeCurrency(cost))
                {
                    shopKeeper.GenerateItems();
                }
            }
        }
    }
}
