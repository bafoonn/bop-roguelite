using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

namespace Pasta
{
    public abstract class PickupBase : MonoBehaviour, IPickup
    {
        [SerializeField]
        private UnityEvent _onPickup = new UnityEvent();
        public UnityEvent OnPickup => _onPickup;

        protected virtual void Awake()
        {
            var rigidbody = this.AddOrGetComponent<Rigidbody2D>();
            rigidbody.isKinematic = true;
            var collider = this.AddOrGetComponent<Collider2D>();
            collider.isTrigger = true;
        }

        public virtual void Take()
        {
            gameObject.Deactivate();
            if (OnPickup != null)
            {
                OnPickup.Invoke();
            }
            StartCoroutine(DestroyRoutine());
        }

        protected virtual IEnumerator DestroyRoutine()
        {
            yield return new WaitForEndOfFrame();
            Destroy(gameObject);
        }
    }
}
