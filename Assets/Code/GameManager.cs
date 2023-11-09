using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pasta
{
    public class GameManager : Singleton<GameManager>
    {
        public override bool DoPersist => true;
        private bool _isPaused = false;

        [SerializeField] private string _mainMenu = "MainMenu";
        [SerializeField] private string _pause = "Pause";
        [SerializeField] private string _game = "Playtest";
        [SerializeField] private string _gameOver = "GameOver";

        [SerializeField, ReadOnly] private string _current = "";

        public static event Action OnPause;
        public static event Action OnUnpause;

        protected override void Init()
        {
            base.Init();
            _current = SceneManager.GetActiveScene().name;
        }

        private void OnEnable()
        {
            InputReader.OnPause += Pause;
            Player.OnPlayerDeath += OnDeath;
        }

        private void OnDisable()
        {
            InputReader.OnPause -= Pause;
            Player.OnPlayerDeath -= OnDeath;
        }

        private void OnDeath()
        {
            SceneManager.LoadSceneAsync(_gameOver, LoadSceneMode.Additive);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(_mainMenu);
            _current = _mainMenu;
        }

        public void Pause()
        {
            if (_current.Equals(_mainMenu)) return;
            if (!_isPaused)
            {
                Time.timeScale = 0;
                SceneManager.LoadSceneAsync(_pause, LoadSceneMode.Additive);
                if (OnPause != null) OnPause();
            }
            else
            {
                if (_current.Equals(_mainMenu)) return;
                Time.timeScale = 1;
                SceneManager.UnloadSceneAsync(_pause);
                if (OnUnpause != null) OnUnpause();
            }
            _isPaused = !_isPaused;
        }

        public void Game()
        {
            SceneManager.LoadSceneAsync(_game);
            _current = _game;
        }
    }
}
