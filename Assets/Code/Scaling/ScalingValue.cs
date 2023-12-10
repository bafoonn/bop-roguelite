using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [System.Serializable]
    public class ScalingValue
    {
        [Range(0, 1)] public float Percentage = 0.1f;
        [HideInInspector] public int Stacks = 1;
        public ScaleType ScaleType;

        public bool Roll()
        {
            return Roll(ScaleType);
        }

        public bool Roll(ScaleType scaleType)
        {
            float random = Random.value;
            float value = GetValue(Stacks, scaleType);
            return random < value;
        }

        public float GetValue(int stacks, ScaleType scaleType)
        {
            float value = 0f;
            switch (scaleType)
            {
                case ScaleType.Linear:
                    value = GetLinear(Percentage, stacks);
                    break;
                case ScaleType.Hyperbolic:
                    value = GetHyperbolic(Percentage, stacks);
                    break;
                case ScaleType.Accumulating:
                    value = GetAccumulating(Percentage, stacks);
                    break;
            }
            return Mathf.Clamp01(value);
        }

        public static float GetLinear(float value, int stacks = 1)
        {
            Mathf.Clamp01(value);
            return Mathf.Clamp01(value * stacks);
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

        public override string ToString()
        {
            return Mathf.RoundToInt(Percentage * 100) + "%";
        }
    }
}
