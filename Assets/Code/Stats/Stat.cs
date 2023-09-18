using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Stat
    {
        private float _baseValue;
        private float _value;
        public float Value
        {
            get
            {
                return _value;
            }

            private set
            {
                if (ValueChanged != null)
                {
                    ValueChanged(value);
                }

                _value = value;
            }
        }

        private HashSet<StatEffect> _effects = new();
        public event Action<float> ValueChanged;

        public Stat(float baseValue)
        {
            _baseValue = baseValue;
            _value = baseValue;
        }

        private float CalculateValue()
        {
            float newValue = _baseValue;

            foreach (StatEffect effect in _effects)
            {
                if (effect.Type == StatEffectType.Additive)
                {
                    newValue += effect.Value;
                }
            }

            foreach (StatEffect effect in _effects)
            {
                if (effect.Type == StatEffectType.Multiplicative)
                {
                    newValue *= effect.Value;
                }
            }

            Value = newValue;
            return newValue;
        }

        public bool AddEffect(StatEffect effect)
        {
            if (!_effects.Add(effect))
            {
                return false;
            }

            CalculateValue();
            return true;
        }

        public bool UpdateEffect(StatEffect effect)
        {
            if (!_effects.Contains(effect))
            {
                return false;
            }

            _effects.Remove(effect);
            _effects.Add(effect);
            CalculateValue();
            return true;
        }

        public bool RemoveEffect(StatEffect effect)
        {
            if (!_effects.Remove(effect))
            {
                return false;
            }

            CalculateValue();
            return true;
        }

        public void Reset()
        {
            _effects.Clear();
            CalculateValue();
        }
    }
}
