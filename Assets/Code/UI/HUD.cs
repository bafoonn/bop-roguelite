using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pasta
{
    public class HUD : Singleton<HUD>
    {
        public class Window
        {
            public readonly string Name;
            public bool IsOpen => _gameObject.activeSelf;
            public readonly bool PauseWhileOpen;
            private readonly GameObject _gameObject;

            public Window(string name, bool pauseWhileOpen, GameObject gameObject)
            {
                Name = name;
                PauseWhileOpen = pauseWhileOpen;
                _gameObject = gameObject;
            }

            public void Open()
            {
                _gameObject.Activate();
            }

            public void Close()
            {
                _gameObject.Deactivate();
            }
        }

        public override bool PersistSceneLoad => false;
        private Window _openWindow = null;
        private List<Window> _windows = new List<Window>();

        private Controls.HUDActions _actions;

        protected override void Init()
        {
            _actions = InputReader.Current.GetHUDActions();
            _actions.Loot.performed += OnLoot;
            _actions.Pause.performed += OnPause;
            _actions.Enable();
        }

        private void OnPause(InputAction.CallbackContext obj)
        {
            OpenWindow("Pause");
        }

        private void OnLoot(InputAction.CallbackContext obj)
        {
            OpenWindow("Loot");
        }

        protected override void OnDestroyed()
        {
            _actions.Loot.performed -= OnLoot;
            _actions.Pause.performed -= OnPause;
        }

        private Window GetWindow(string name)
        {
            foreach (var window in _windows)
            {
                if (window.Name == name) return window;
            }
            return null;
        }

        public void OpenWindow(string name)
        {
            var window = GetWindow(name);
            if (window == null) return;
            if (_openWindow != null) return;
            window.Open();
            if (window.PauseWhileOpen)
            {
                GameManager.Current.Pause();
            }
            _openWindow = window;
        }

        public void CloseWindow()
        {
            if (_openWindow == null) return;
            _openWindow.Close();
            if (_openWindow.PauseWhileOpen)
            {
                GameManager.Current.Unpause();
            }
            _openWindow = null;
        }
    }
}
