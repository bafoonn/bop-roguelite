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
        protected List<T> _objects = new();
        public List<T> Objects => new List<T>(_objects);
        public LayerMask SensedLayers;

        public UnityEvent<T> OnItemDetected;

        protected virtual void Awake()
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

            if (collision.TryGetComponent<T>(out var obj))
            {
                if (_objects.Contains(obj)) return;
                _objects.Add(obj);
                if (OnItemDetected != null)
                {
                    OnItemDetected.Invoke(obj);
                }
                OnDetect(obj);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<T>(out var obj))
            {
                _objects.Remove(obj);
                OnUndetect(obj);
            }
        }

        protected virtual void OnDetect(T obj)
        {
        }

        protected virtual void OnUndetect(T obj)
        {
        }
    }
}
