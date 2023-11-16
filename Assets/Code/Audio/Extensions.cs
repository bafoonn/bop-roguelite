using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public static partial class Extensions
    {
        public static float FromDBToLinear(this float db)
        {
            return Mathf.Clamp01(Mathf.Pow(10.0f, db / 20.0f));
        }

        public static float FromLinearToDB(this float linear)
        {
            return linear <= 0 ? -80f : Mathf.Log10(linear) * 20.0f;
        }
    }
}
