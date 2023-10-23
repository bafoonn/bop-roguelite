using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pasta
{
    public class ItemPickup : PickupBase
    {
        [field: SerializeField] public ItemBase Item { get; private set; }


        private void Start()
        {
            Setup(Item, hasCost);
        }

        public void Setup(ItemBase item, bool isShopItem)
        {
            if (item == null)
            {
                gameObject.Deactivate();
                return;
            }
            hasCost = isShopItem;
            gameObject.name = item.Name;
            GetComponent<SpriteRenderer>().sprite = item.Sprite;
            Item = item;
            gameObject.Activate();
        }

        public override void Take()
        {
            if (!Item.CanLoot)
            {
                return;
            }

            if (hasCost)
            {
                ShopItemGeneration shop = GetComponentInParent<ShopItemGeneration>();
                shop.ItemBought();
            }

            gameObject.Deactivate();
            if (OnPickup != null)
            {
                OnPickup.Invoke(this);
                ItemsUI.Current.Add(Item); // FOR INVENTORY
            }
        }
        public bool CheckIfShopItem()
        {
            return hasCost;
        }
    }
}
