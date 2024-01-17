using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pasta
{
    public class HUDWindow : MonoBehaviour
    {
        public string Name => gameObject.name;
        public bool PauseWhileOpen = false;
        public bool IsOpen => gameObject.activeSelf;
        public UnityEvent OnContinue;
        public UnityEvent OnEscape;

        public void Open()
        {
            gameObject.Activate();
        }

        public void Close()
        {
            gameObject.Deactivate();
        }
    }
}
