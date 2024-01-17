using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pasta
{
    public class HUD : Singleton<HUD>
    {
        public override bool PersistSceneLoad => false;
        private HUDWindow _openWindow = null;
        private List<HUDWindow> _windows = new List<HUDWindow>();

        private Controls.HUDActions _hudActions;

        public bool HasWindowOpen => _openWindow != null;

        protected override void Init()
        {
            _hudActions = InputReader.Current.GetHUDActions();
            _hudActions.Enable();
            _hudActions.Loot.performed += OnLoot;
            _hudActions.Pause.performed += OnPause;


            var windows = GetComponentsInChildren<HUDWindow>(true);
            foreach (var window in windows)
            {
                AddWindow(window);
            }
        }

        private void OnPause(InputAction.CallbackContext obj)
        {
            OpenWindow("Pause");
        }

        private void OnLoot(InputAction.CallbackContext obj)
        {
            if (_openWindow != null && _openWindow.Name.Equals("Loot"))
            {
                CloseWindow();
            }
            else
            {
                OpenWindow("Loot");
            }

        }

        protected override void OnDestroyed()
        {
            _hudActions.Loot.performed -= OnLoot;
            _hudActions.Pause.performed -= OnPause;
        }

        private void AddWindow(HUDWindow window)
        {
            if (_windows.Contains(window)) return;
            if (window.Name == string.Empty) return;
            _windows.Add(window);
            window.Close();
        }

        private HUDWindow GetWindow(string name)
        {
            foreach (var window in _windows)
            {
                if (window.Name.Equals(name)) return window;
            }
            return null;
        }

        public HUDWindow OpenWindow(string name, bool closeOpen = false)
        {
            var window = GetWindow(name);
            if (window == null) return null;
            if (closeOpen == false && _openWindow != null) return null;
            else CloseWindow();
            window.Open();
            if (window.PauseWhileOpen)
            {
                GameManager.Current.Pause();
            }
            _openWindow = window;
            return window;
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
