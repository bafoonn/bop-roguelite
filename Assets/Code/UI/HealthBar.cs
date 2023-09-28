using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pasta
{
    [RequireComponent(typeof(Image))]
    public class HealthBar : MonoBehaviour
    {
        private Health _health;
        [SerializeField] private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            Assert.IsNotNull(_image);
            _image.type = Image.Type.Filled;
            _image.fillMethod = Image.FillMethod.Horizontal;
            _image.color = Color.red;
        }

        public void Setup(Health health)
        {
            _health = health;
            _health.OnHealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(float value)
        {
            _image.fillAmount = value / _health.MaxHealth;
        }

        private void OnDestroy()
        {
            _health.OnHealthChanged -= OnHealthChanged;
        }
    }
}
