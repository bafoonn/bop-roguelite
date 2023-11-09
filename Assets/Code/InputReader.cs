using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class InputReader : Singleton<InputReader>
    {
        public override bool DoPersist => true;
        private Controls _controls;

        public static event Action OnPause;

        protected override void Init()
        {
            _controls = new Controls();
            _controls.Enable();
            _controls.Function.Pause.performed += (context) =>
            {
                Debug.Log("Pause pressed");
                if (OnPause != null) OnPause();
            };
        }

        public Controls.PlayerActions GetPlayerActions()
        {
            return _controls.Player;
        }
    }
}
