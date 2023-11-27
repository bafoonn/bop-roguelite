using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pasta
{
    public class ItemPickup : PickupBase
    {
        [field: SerializeField] public ItemBase Item { get; private set; }
        private bool hasCost = false;
        private Level level;

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
            level = GetComponentInParent<Level>();
            gameObject.Activate();
        }

        public override void Take()
        {
            base.Take();

            if (hasCost)
            {
                ShopItemGeneration shop = GetComponentInParent<ShopItemGeneration>();
                shop.ItemBought();
            }
            else if(level != null)
            {
                level.PickedUpReward();
            }
            ItemsUI.Current.Add(Item); // FOR INVENTORY
        }
        public bool CheckIfShopItem()
        {
            return hasCost;
        }
    }
}
