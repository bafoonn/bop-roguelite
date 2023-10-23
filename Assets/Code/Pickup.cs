using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pasta
{
    public class Pickup : MonoBehaviour
    {
        [field: SerializeField] public ItemBase Item { get; private set; }

        public UnityEvent<Pickup> OnPickup;
        private bool hasCost = false;

        private void Awake()
        {
            var rigidbody = this.AddOrGetComponent<Rigidbody2D>();
            rigidbody.isKinematic = true;
            var collider = this.AddOrGetComponent<Collider2D>();
            collider.isTrigger = true;
        }

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

        public void Take()
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
