using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelper
{
    public static Vector3[] CalculateBezierPoints(Vector3 p0, Vector3 p1, Vector3 p2, int nbrPoints)
    {
        Vector3[] points = new Vector3[nbrPoints + 1];
        for (int i = 0; i <= nbrPoints; i++)
            points[i] = CalculateQuadraticBezierPoint((float)i / (float)nbrPoints, p0, p1, p2);
        return points;
    }

    public static Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }
}
