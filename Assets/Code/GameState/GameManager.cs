using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pasta
{
    public class GameManager : Singleton<GameManager>
    {
        public override bool DoPersist => true;
        private bool _isPaused = false;

        public bool IsPaused => _isPaused;

        [SerializeField] private string _mainMenuScene = "MainMenu";
        [SerializeField] private string _pauseScene = "Pause";
        [SerializeField] private string _gameScene = "Playtest";
        [SerializeField] private string _gameOverScene = "GameOver";

        [field: SerializeField] public GameStateType CurrentState { get; private set; }

        private GameState _currentState = null;
        private GameState _prevState = null;

        private GameState _mainMenuState = null;
        private GameState _gameState = null;
        private GameState _pauseState = null;
        private GameState _gameOverState = null;
        private GameState[] _gameStates = null;

        public static event Action OnPause;
        public static event Action OnUnpause;

        protected override void Init()
        {
            base.Init();
            _mainMenuState = new GameState(_mainMenuScene, GameStateType.MainMenu, false, () => true);
            _gameState = new GameState(_gameScene, GameStateType.Game, false, () => true);
            _pauseState = new GameState(_pauseScene, GameStateType.Pause, true,
                canActivate: () => _currentState.SceneName.Equals(_gameScene),
                onActivate: () =>
                {
                    _isPaused = true;
                    Time.timeScale = 0;
                    if (OnPause != null) OnPause();
                },
                onDeactivate: () =>
                {
                    _isPaused = false;
                    Time.timeScale = 1;
                    if (OnUnpause != null) OnUnpause();
                });

            _gameOverState = new GameState(_gameOverScene, GameStateType.GameOver, true,
                canActivate: () => _currentState.SceneName.Equals(_gameScene));

            _gameStates = new GameState[]
            {
                _mainMenuState,
                _gameState,
                _pauseState,
                _gameOverState
            };

            foreach (var state in _gameStates)
            {
                if (SceneManager.GetActiveScene().name == state.SceneName)
                {
                    _currentState = state;
                    _currentState.Activate();
                    CurrentState = _currentState.Type;
                    break;
                }
            }
        }

        private void OnEnable()
        {
            InputReader.OnPause += OnPaused;
            Player.OnPlayerDeath += OnDeath;
        }

        private void OnDisable()
        {
            InputReader.OnPause -= OnPaused;
            Player.OnPlayerDeath -= OnDeath;
        }

        public bool GoTo(GameStateType type, bool forceLoad = false)
        {
            if (_currentState == null) return false;
            if (_currentState.Type == type)
            {
                Debug.LogWarning($"{type} state is already active.");
                return false;
            }

            GameState target = null;

            foreach (var state in _gameStates)
            {
                if (state.Type == type)
                {
                    target = state;
                    break;
                }
            }

            if (target == null)
            {
                Debug.LogWarning($"GameState {type} is missing.");
                return false;
            }

            if (!target.CanActivate)
            {
                Debug.LogWarning($"State {type} couldn't activate.");
                return false;
            }

            _currentState.Deactivate();
            _prevState = _currentState;
            target.Activate(forceLoad);
            _currentState = target;
            CurrentState = _currentState.Type;

            return true;
        }

        public void GoBack()
        {
            if (_prevState != null)
            {
                GoTo(_prevState.Type);
            }
        }

        private void OnDeath()
        {
            GoTo(GameStateType.GameOver);
        }

        public void OnPaused()
        {
            if (!_isPaused)
            {
                GoTo(GameStateType.Pause);
            }
            else
            {
                GoBack();
            }
        }


    }
}
