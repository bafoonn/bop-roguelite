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
        [SerializeField] private float _currentHealth = 100;
        [SerializeField] private GameObject damageText;
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
        public bool IsMaxHealth => CurrentHealth == MaxHealth;
        public event Action OnDeath;
        public event Action<float> OnHealthChanged;

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

            CurrentHealth -= damage;
            Vector3 Position = transform.position;
            if(damageText != null)
            {
                GameObject DamageTextSpawned = Instantiate(damageText, Position, transform.localRotation);
                DamageTextSpawned.transform.GetChild(0).GetComponent<Text>().text = damage.ToString();
            }
            
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
        }
    }
}
