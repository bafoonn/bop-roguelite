using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pasta
{
    public class FirstSelectUIObject : MonoBehaviour
    {
        private void OnEnable()
        {
            Select();
        }

        private void Start()
        {
            Select();
        }

        public void Select()
        {
            var eventSystem = EventSystem.current;
            if (eventSystem != null)
            {
                eventSystem.SetSelectedGameObject(gameObject);
            }
        }
    }
}
