using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public struct Line
    {
        const float verticalLineGradient = 1e5f;
        float gradient;
        float yIntercept;
        float gradientPerpendicular;
        Vector2 pointOnLine_1;
        Vector2 pointOnLine_2;
        bool approachSide;

        public Line(Vector2 pointOnTheLine, Vector2 PerpendicularToLine)
		{
            float dx = pointOnTheLine.x - PerpendicularToLine.x;
            float dy = pointOnTheLine.y - PerpendicularToLine.y;

            if(dx == 0)
			{
                gradientPerpendicular = verticalLineGradient;
			}
			else
			{
                gradientPerpendicular = dy / dx;
            }

            if(gradientPerpendicular == 0)
			{
                gradient = verticalLineGradient;
			}
			else
			{
                gradient = -1 / gradientPerpendicular;
            }

            yIntercept = pointOnTheLine.y - gradient * pointOnTheLine.x;
            pointOnLine_1 = pointOnTheLine;
            pointOnLine_2 = pointOnTheLine + new Vector2(1, gradient);

            approachSide = false;
            approachSide = GetSide(PerpendicularToLine);
            
		}

        bool GetSide(Vector2 point)
		{
            return (point.x - pointOnLine_1.x) * (pointOnLine_2.y - pointOnLine_1.y) > (point.y - pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
		}

        public bool hasCrossedLine(Vector2 point)
		{
            return GetSide(point) != approachSide;
		}

        public float DistanceFromPoint(Vector2 p)
        {
            float yInterceptPerpendicular = p.y - gradientPerpendicular * p.x;
            float intersectX = (yInterceptPerpendicular - yIntercept) / (gradient - gradientPerpendicular);
            float intersectY = gradient * intersectX + yIntercept;
            return Vector2.Distance(p, new Vector2(intersectX, intersectY));
        }

        public void DrawWithGizmos(float lenght)
		{
            Vector3 lineDIr = new Vector3(1, 0, gradient).normalized;
            Vector3 lineCenter = new Vector3(pointOnLine_1.x, 0, pointOnLine_1.y) + Vector3.forward;
            Gizmos.DrawLine(lineCenter - lineDIr * lenght / 2f, lineCenter + lineDIr * lenght / 2f);
		}
    }
}
