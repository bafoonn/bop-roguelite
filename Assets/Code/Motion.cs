using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Motion
{
    public enum MotionType
    {
        Continuous,
        Single,
        Additive
    }

    public Vector2 Direction = Vector2.zero;
    public MotionType Type = MotionType.Continuous;
    public float Modifier = 1.0f;
    public bool Enabled = true;
}
