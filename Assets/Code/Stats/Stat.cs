using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [Serializable]
    public class Stat
    {
        [SerializeField] private float _baseValue;
        [SerializeField] private StatType _type;
        public float BaseValue => _baseValue;
        public StatType Type => _type;

        private HashSet<StatEffect> _effects = new HashSet<StatEffect>();

        public Stat(float baseValue)
        {
            _baseValue = baseValue;
        }

        public Stat(StatType type)
        {
            _type = type;
        }

        public Stat(float baseValue, StatType type)
        {
            _baseValue = baseValue;
            _type = type;
        }

        public float Value
        {
            get
            {
                float value = BaseValue;

                foreach (StatEffect effect in _effects)
                {
                    if (effect.Type == StatEffectType.Additive)
                    {
                        value += effect.Value;
                    }
                }

                foreach (StatEffect effect in _effects)
                {
                    if (effect.Type == StatEffectType.Multiplicative)
                    {
                        value *= effect.Value;
                    }
                }

                return value;
            }
        }

        public void AddEffect(StatEffect effect) => _effects.Add(effect);
        public void RemoveEffect(StatEffect effect) => _effects.Remove(effect);
    }
}
