using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class StatusUI : MonoBehaviour
    {
        private StatusHandler _handler = null;
        private StatusIconUI[] _icons;

        public void Setup(StatusHandler statusHandler)
        {
            _handler = statusHandler;
            _handler.OnApply += OnApply;
            _handler.OnUnapply += OnUnapply;
            _icons = GetComponentsInChildren<StatusIconUI>(true);
        }

        private void OnDestroy()
        {
            if (_handler != null)
            {
                _handler.OnApply -= OnApply;
                _handler.OnUnapply -= OnUnapply;
            }
        }

        private void OnApply(StatusType type)
        {
            foreach (var icon in _icons)
            {
                if (icon.StatusType != type) continue;
                icon.Add();
            }
        }

        private void OnUnapply(StatusType type)
        {
            foreach (var icon in _icons)
            {
                if (icon.StatusType != type) continue;
                icon.Remove();
            }
        }
    }
}
