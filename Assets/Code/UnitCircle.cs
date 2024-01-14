using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class UnitCircle : MonoBehaviour
    {
        public float degrees = 0, distance = 1f, degreeOffset = 0f;
        public Transform Pivot;
        public Transform Target;

        public void SetPosition()
        {
            SetPosition(degrees, distance, degreeOffset);
        }

        public void SetPosition(float degrees, float distance = 1, float degreeOffset = 0)
        {
            var pivot = Pivot != null ? Pivot : transform.parent;
            if (pivot == null) return;
            var target = Target != null ? Target : transform;
            target.position = pivot.position + (Vector3)Point(degrees, distance, degreeOffset);
        }


        public static float Degrees(Vector2 point)
        {
            return Radians(point) * Mathf.Rad2Deg;
        }

        public static float Radians(Vector2 point)
        {
            point.Normalize();
            return Mathf.Atan2(point.y, point.x);
        }

        /// <summary>
        /// Calculates a point on a unit circle.
        /// </summary>
        /// <param name="degrees">The degree of the point.</param>
        /// <param name="distance">Multiplier applied to the return vector.</param>
        /// <param name="degreeOffset">Added to degrees.</param>
        /// <returns>A point on a unit circle.</returns>
        public static Vector2 Point(float degrees, float distance = 1, float degreeOffset = 0)
        {
            float cos = Mathf.Cos((degrees + degreeOffset) * Mathf.Deg2Rad);
            float sin = Mathf.Sin((degrees + degreeOffset) * Mathf.Deg2Rad);
            return new Vector2(cos, sin) * distance;
        }

        /// <summary>
        /// Interpolates between min- and maxDegrees by t. 
        /// </summary>
        /// <param name="minDegrees">The start value.</param>
        /// <param name="maxDegrees">The end value.</param>
        /// <param name="t">Interpolation between min- and maxDegrees.</param>
        /// <param name="distance"></param>
        /// <param name="degreeOffset">Degrees by which the interpolated value is offset by.</param>
        /// <returns>A point on a unit circle using the interpolated degrees.</returns>
        public static Vector2 Lerp(float minDegrees, float maxDegrees, float t, float distance = 1, float degreeOffset = 0)
        {
            float value = Mathf.Lerp(minDegrees, maxDegrees, t);
            return Point(value, distance, degreeOffset);
        }
    }
}
