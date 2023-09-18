using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class Health : MonoBehaviour
    {
        public bool IsDead { get; private set; } = false;
        [field: SerializeField] public float MaxHealth { get; private set; } = 100;
        [field: SerializeField] public float CurrentHealth { get; private set; }
        public bool isMaxHealth => CurrentHealth == MaxHealth;
        public event Action OnDeath;
        public event Action OnDamaged;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Assert(OnDeath != null, $"{transform.name} {name} has not been setup.");
        }
        public void Reset()
        {
            CurrentHealth = MaxHealth;
        }
        public void TakeDamage(float damage)
        {
            if (IsDead) return;

            CurrentHealth -= damage;
            if (OnDamaged != null)
            {
                OnDamaged();
            }

            if (CurrentHealth <= 0)
            {
                if (OnDeath != null)
                    OnDeath();
                IsDead = true;
            }
        }
    }
