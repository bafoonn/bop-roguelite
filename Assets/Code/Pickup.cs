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

        private void Awake()
        {
            var rigidbody = this.AddOrGetComponent<Rigidbody2D>();
            rigidbody.isKinematic = true;
            var collider = this.AddOrGetComponent<Collider2D>();
            collider.isTrigger = true;
        }

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

        public void Take()
        {
            if (!Item.CanLoot)
            {
                return;
            }

            gameObject.Deactivate();
            if (OnPickup != null)
            {
                OnPickup.Invoke(this);
                ItemsUI.Current.Add(Item); // FOR INVENTORY
            }
        }
    }
}
