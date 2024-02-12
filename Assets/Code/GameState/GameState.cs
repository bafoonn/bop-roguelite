using System;
using System.Diagnostics;
using UnityEngine.SceneManagement;

namespace Pasta
{
    public class GameState
    {
        public readonly string SceneName;
        public readonly GameStateType Type;
        public bool IsActive { get; private set; }
        public readonly bool IsAdditive;
        private readonly LoadSceneMode _mode;
        private readonly Action _onActivate;
        private readonly Action _onDeactivate;
        private readonly Func<bool> _canActivate;
        public bool CanActivate => _canActivate() || IsActive == true;

        public GameState(string sceneName, GameStateType type, bool isAdditive, Func<bool> canActivate, Action onActivate = null, Action onDeactivate = null)
        {
            SceneName = sceneName;
            Type = type;
            IsAdditive = isAdditive;
            _mode = isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            _onActivate = onActivate;
            _onDeactivate = onDeactivate;
            _canActivate = canActivate;
        }

        public bool Activate()
        {
            if (IsActive) return false;
            IsActive = true;

            if (!SceneManager.GetActiveScene().name.Equals(SceneName))
            {
                SceneManager.LoadSceneAsync(SceneName, _mode);
            }
            if (_onActivate != null) _onActivate();
            return true;
        }

        public bool Reload()
        {
            if (!IsActive)
            {
                UnityEngine.Debug.LogWarning($"Cannot reload state, state ({Type}) is not active.");
                return false;
            }
            if (_mode == LoadSceneMode.Additive)
            {
                UnityEngine.Debug.LogWarning("Cannot reload state, state scene is additive.");
                return false;
            }

            SceneManager.LoadScene(SceneName, _mode);

            return true;
        }

        public void Deactivate()
        {
            if (!IsActive) return;
            IsActive = false;

            if (IsAdditive) SceneManager.UnloadSceneAsync(SceneName);
            if (_onDeactivate != null) _onDeactivate();
        }
    }
}
