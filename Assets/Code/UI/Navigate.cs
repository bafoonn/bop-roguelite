using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Navigate : MonoBehaviour
    {
        public void Restart()
        {
            GameManager.Current.Game();
        }

        public void MainMenu()
        {
            GameManager.Current.MainMenu();
        }

        public void Pause()
        {
            GameManager.Current.Pause();
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
