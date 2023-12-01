using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Stat
    {
        public readonly StatType Type;
        private float _baseValue;
        private float _value;
        private float _unclampedValue;
        private readonly float _minValue = 0;
        private readonly float _maxValue = 0;

        public float Value
        {
            get
            {
                if (_maxValue > 0)
                {
                    return Mathf.Clamp(_value, 0, _maxValue);
                }
                return _value;
            }

            private set
            {

                _unclampedValue = value;
                _value = Mathf.Clamp(_unclampedValue, _minValue, _maxValue > 0 ? _maxValue : _unclampedValue);
                if (ValueChanged != null)
                {
                    ValueChanged(_value);
                }
            }
        }

        //private HashSet<StatEffect> _effects = new();
        private List<StatEffect> _effects = new();
        public event Action<float> ValueChanged;

        public Stat(float baseValue, StatType type)
        {
            _baseValue = baseValue;
            _value = baseValue;
            Type = type;
        }

        public Stat(float value, StatType type, float minValue = 0, float maxValue = 0)
        {
            _baseValue = value;
            _value = value;
            Type = type;
            _minValue = minValue;
            _maxValue = maxValue;
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
            //if (!_effects.Add(effect))
            //{
            //    return false;
            //}
            _effects.Add(effect);
            CalculateValue();
            return true;
        }

        public bool UpdateEffect(StatEffect effect)
        {
            //if (!_effects.Contains(effect))
            //{
            //    return false;
            //}

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
