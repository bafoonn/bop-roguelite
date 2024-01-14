using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Cone : MonoBehaviour
    {
        public PolygonCollider2D Collider;
        [Range(0.5f, 10)] public float Range = 2;
        [Range(0, 180)] public float Angle = 90;
        [Range(3, 21)] public int Divisions = 7;

        private void Awake()
        {
            enabled = false;
        }

        private void OnValidate()
        {
            CreateCone();
        }

        public void CreateCone()
        {
            if (!enabled) return;
            if (Collider == null) return;
            if (Divisions < 3) Divisions = 3;

            Vector2[] points = new Vector2[Divisions + 1];
            float min = Angle / 2;
            float max = -min;
            for (int i = 0; i < Divisions; i++)
            {
                float t = (float)i / (float)(Divisions - 1);
                points[i] = UnitCircle.Lerp(min, max, t) * Range;
            }

            Collider.points = points;
            Collider.SetPath(0, points);
        }
    }
}
