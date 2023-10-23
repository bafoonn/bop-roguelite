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
            Setup(Item);
        }

        public void Setup(ItemBase item)
        {
            if (item == null)
            {
                gameObject.Deactivate();
                return;
            }

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
            base.Take();
            ItemsUI.Current.Add(Item); // FOR INVENTORY
        }
    }
}
