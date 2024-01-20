using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pasta
{
    public class HUD : Singleton<HUD>
    {
        public override bool PersistSceneLoad => false;
        private HUDWindow _currentWindow = null;
        private List<HUDWindow> _windows = new List<HUDWindow>();

        private Controls.HUDActions _hudActions;

        public bool HasWindowOpen => _currentWindow != null;
        public static event Action<HUDWindow> OnOpenWindow;
        public static event Action OnCloseWindow;

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
            if (_currentWindow != null && _currentWindow.Name.Equals("Loot"))
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
            if (closeOpen == false && _currentWindow != null) return null;
            else CloseWindow();
            window.Open();
            if (window.PauseWhileOpen)
            {
                GameManager.Current.Pause();
            }
            if (OnOpenWindow != null)
            {
                OnOpenWindow(window);
            }
            _currentWindow = window;
            return window;
        }

        public void CloseWindow()
        {
            if (_currentWindow == null) return;
            _currentWindow.Close();
            if (_currentWindow.PauseWhileOpen)
            {
                GameManager.Current.Unpause();
            }
            if (OnCloseWindow != null)
            {
                OnCloseWindow();
            }
            _currentWindow = null;
        }
    }
}
