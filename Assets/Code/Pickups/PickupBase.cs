using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pasta
{
    public abstract class PickupBase : MonoBehaviour, IPickup
    {
        [SerializeField]
        private UnityEvent _onPickup = new UnityEvent();
        public UnityEvent OnPickup => _onPickup;

        public virtual void Take()
        {
            gameObject.Deactivate();
            if (OnPickup != null)
            {
                OnPickup.Invoke();
            }
            Destroy(gameObject);
        }
    }
}
