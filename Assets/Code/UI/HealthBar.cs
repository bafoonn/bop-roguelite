using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pasta
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _text;

        private Health _health;
        private float _current = 0;
        public float Current => _current;
        public Health Health => _health;

        private void Awake()
        {
            Assert.IsNotNull(_image, "HealthBar has no image set.");
            _image.type = Image.Type.Filled;
        }

        public void SetHealth(Health health)
        {
            if (_health != null)
            {
                _health.OnHealthChanged -= OnHealthChanged;
            }

            _health = health;
            _health.OnHealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(float value)
        {
            if (_text != null)
            {
                _text.text = Mathf.RoundToInt(value).ToString();
            }
            _current = value / _health.MaxHealth;
            _image.fillAmount = _current;
        }

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.OnHealthChanged -= OnHealthChanged;
            }
        }
    }
}
