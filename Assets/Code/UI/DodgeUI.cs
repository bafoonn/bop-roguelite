using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pasta
{
    public class DodgeUI : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _text;
        private PlayerMovement _playerMovement;
        private UIElementAnimations _elementAnimations;

        private void Awake()
        {
            _elementAnimations = GetComponent<UIElementAnimations>();
            _playerMovement = GetComponentInParent<PlayerMovement>();
            _playerMovement.OnDodgeGained.AddListener((_) => _elementAnimations.Highlight(1, 1));
        }

        private void Update()
        {
            _image.type = _playerMovement.IsDodgeRecharging ? Image.Type.Filled : Image.Type.Simple;
            _image.fillAmount = _playerMovement.DodgeCooldownProgress;
            if (_text != null) _text.text = _playerMovement.DodgeCount.ToString();
        }
    }
}
