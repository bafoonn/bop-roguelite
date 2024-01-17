using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pasta
{
    public class InputReader : Singleton<InputReader>
    {
        public override bool PersistSceneLoad => true;
        private Controls _controls;

        public static event Action OnPause;

        protected override void Init()
        {
            _controls = new Controls();
            _controls.Enable();
        }

        public Controls.PlayerActions GetPlayerActions() => _controls.Player;
        public Controls.HUDActions GetHUDActions() => _controls.HUD;
        public Controls.UIActions GetUIActions() => _controls.UI;

    }
}
