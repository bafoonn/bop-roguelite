using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pasta
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Sensor<T> : MonoBehaviour
    {
        private Rigidbody2D _rigidbody = null;
        private List<T> _items = new();
        public List<T> Items => _items;
        public LayerMask SensedLayers;

        public UnityEvent<T> OnItemDetected;

        private void Awake()
        {
            if (TryGetComponent<Collider2D>(out var collider))
            {
                collider.isTrigger = true;
            }
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.isKinematic = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!SensedLayers.Includes(collision.gameObject.layer))
            {
                return;
            }

            if (collision.TryGetComponent<T>(out var item))
            {
                _items.Add(item);
                if (OnItemDetected != null)
                {
                    OnItemDetected.Invoke(item);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<T>(out var item))
            {
                _items.Remove(item);
            }
        }
    }
}
