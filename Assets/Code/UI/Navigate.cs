using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Navigate : MonoBehaviour
    {
        public void Game()
        {
            GameManager.Current.GoTo(GameStateType.Game);
        }

        public void ReloadGame()
        {
            GameManager.Current.GoTo(GameStateType.Game, forceLoad: true);
        }

        public void MainMenu()
        {
            GameManager.Current.GoTo(GameStateType.MainMenu);
        }

        //public void Pause()
        //{
        //    GameManager.Current.OnPaused();
        //}

        public void Exit()
        {
            Application.Quit();
        }
    }
}
