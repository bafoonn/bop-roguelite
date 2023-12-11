using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Pasta
{
    /// <summary>
    /// A struct used to modify the value of a Stat
    /// </summary>
    [Serializable]
    public class StatEffect : Effect
    {
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

        public StatEffect(string name, StatType statType, float value, StatEffectType type) : base()
        {
            Name = name;
            Stat = statType;
            Value = value;
            Type = type;
        }

        public override void Apply()
        {
            base.Apply();
            StatManager.Current.AddEffectToStat(this);
        }

        public override void Unapply()
        {
            base.Unapply();
            StatManager.Current.RemoveEffectFromStat(this);
        }

        public override string ToString()
        {
            switch (Type)
            {
                case StatEffectType.Additive:
                    {
                        char character = Value > 0 ? '+' : '-';
                        return $"{character}{Mathf.Abs(Value)} {Stat.ReadableStr()}";
                    }
                case StatEffectType.Multiplicative:
                    {
                        string effect = Value > 1 ? "increased" : "reduced";
                        string value = Mathf.Abs(Mathf.RoundToInt((Value - 1) * 100)).ToString() + "%";
                        return $"{value} {effect} {Stat.ReadableStr()}";
                    }
                default: return "";
            }
        }
    }
}
