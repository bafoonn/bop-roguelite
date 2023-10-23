using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pasta
{
    public class CurrencyUI : MonoBehaviour
    {
        private Player _player = null;
        [SerializeField] private Text _text;

        public void Setup(Player player)
        {
            if (_text == null) return;
            _player = player;
        }

        private void Update()
        {
            if (_player != null)
            {
                _text.text = _player.currency.ToString();
            }
        }
    }
}
