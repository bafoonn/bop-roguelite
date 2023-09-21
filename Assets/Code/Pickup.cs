using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pasta
{
    public class Pickup : MonoBehaviour
    {
        public Item Item;

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

        public void Setup(Item item)
        {
            if (item == null)
            {
                gameObject.Deactivate();
                return;
            }

            gameObject.name = item.Name;
            GetComponent<SpriteRenderer>().sprite = item.Sprite;
            gameObject.Activate();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out _))
            {
                gameObject.Deactivate();
                Item.Loot();
                if (OnPickup != null)
                {
                    OnPickup.Invoke(this);
                }
            }
        }
    }
}
