using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    /// <summary>
    /// A struct used to modify the value of a Stat
    /// </summary>
    [Serializable]
    public struct StatEffect : IEquatable<StatEffect>
    {
        public readonly Guid Id;
        /// <summary>
        /// The name of the effect, also used in comparing. Has to be unique
        /// </summary>
        public string Name;
        public StatType Stat;
        public float Value;
        /// <summary>
        /// Defines if the value of this effect is added to the base value of a stat <br></br>or if the base value is multiplied by the value of this effect
        /// </summary>
        public StatEffectType Type;

        public StatEffect(string name, StatType statType, float value, StatEffectType type)
        {
            Id = new Guid();
            Name = name;
            Stat = statType;
            Value = value;
            Type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is StatEffect)) return false;
            return Equals((StatEffect)obj);
        }

        public bool Equals(StatEffect other)
        {
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"StatEffect(name: {Name}, stat: {Stat}, value: {Value}, type: {Type})";
        }

    }
}
