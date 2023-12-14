using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [System.Serializable]
    public struct ScalingValue
    {
        [Range(0, 1f)] public float Value;
        public ScaleType ScaleType;

        public bool Roll(int stacks)
        {
            return Roll(Value, stacks, ScaleType);
        }

        public float GetValue(int stacks)
        {
            return GetValue(Value, stacks, ScaleType);
        }

        public static bool Roll(float value, int stacks, ScaleType scaleType)
        {
            float random = Random.value;
            return random < GetValue(value, stacks, scaleType);
        }

        public static float GetValue(float value, int stacks, ScaleType scaleType)
        {
            switch (scaleType)
            {
                case ScaleType.Linear: return GetLinear(value, stacks);
                case ScaleType.Hyperbolic: return GetHyperbolic(value, stacks);
                case ScaleType.Accumulating: return GetAccumulating(value, stacks);
                case ScaleType.Static: return value;
                default: return 0;
            }
        }

        public static float GetLinear(float value, int stacks = 1)
        {
            return value * stacks;
        }

        public static float GetHyperbolic(float value, int stacks = 1)
        {
            Mathf.Clamp01(value);
            return 1 - 1 / (1 + value * stacks);
        }

        public static float GetAccumulating(float value, int stacks = 1)
        {
            value = Mathf.Clamp01(value);
            return 1 - Mathf.Pow(1 - value, stacks);
        }
    }
}
