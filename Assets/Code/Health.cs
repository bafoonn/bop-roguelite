using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pasta
{
    public class Health : MonoBehaviour
    {
        public bool IsDead => CurrentHealth <= 0;
        [field: SerializeField] public float MaxHealth { get; private set; } = 100;
        [SerializeField] private float _currentHealth = 0;
        [SerializeField] private GameObject damageText;
        public bool immune = false;
        public float CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                if (OnHealthChanged != null)
                {
                    OnHealthChanged(_currentHealth);
                }
            }
        }
        public bool IsMaxHealth => CurrentHealth >= MaxHealth;
        public event Action OnDeath;
        public event Action OnDamaged;
        public event Action OnHealed;
        public event Action<float> OnHealthChanged;

        private void Awake()
        {
            _currentHealth = MaxHealth;
        }

        public void SetMax(float value)
        {
            float t = CurrentHealth / MaxHealth;
            MaxHealth = value;
            CurrentHealth = value * t;
        }

        public void Reset()
        {
            CurrentHealth = MaxHealth;
        }

        public virtual void TakeDamage(float damage)
        {
            if (IsDead) return;
            if (immune) return;
            CurrentHealth -= damage;
            Vector2 Position = (Vector2)transform.position + Vector2.up + UnityEngine.Random.insideUnitCircle;
            if (damageText != null)
            {
                GameObject DamageTextSpawned = Instantiate(damageText, Position, transform.localRotation);
                DamageTextSpawned.GetComponentInChildren<Text>().text = damage.ToString("#");
            }

            if (OnDamaged != null) OnDamaged();

            if (CurrentHealth <= 0)
            {
                if (OnDeath != null)
                    OnDeath();
            }
        }

        public virtual void Heal(float amount)
        {
            if (IsMaxHealth) return;
            CurrentHealth += amount;
            if (OnHealed != null) OnHealed();
        }
    }
}
