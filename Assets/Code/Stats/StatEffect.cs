using System;
using System.Collections.Generic;

namespace Pasta
{
    [Serializable]
    public struct StatEffect : IEqualityComparer<StatEffect>
    {
        public readonly string Name;
        public float Value;
        public readonly StatEffectType Type;
        public StatEffect(string name, float value, StatEffectType type)
        {
            Name = name;
            Value = value;
            Type = type;
        }

        public bool Equals(StatEffect x, StatEffect y)
        {
            return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(StatEffect obj)
        {
            return obj.GetHashCode();
        }
    }
}
