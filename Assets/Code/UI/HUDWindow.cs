using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pasta
{
    public class HUDWindow : MonoBehaviour
    {
        public HUD.Window Window;
        public string Name => Window.ToString();
        public bool PauseWhileOpen = false;
        public bool IsOpen => gameObject.activeSelf;
        public UnityEvent OnContinue;
        public UnityEvent OnEscape;

        private void OnValidate()
        {
            gameObject.name = Name;
        }

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
