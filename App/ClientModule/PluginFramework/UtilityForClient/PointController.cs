using UnityEngine;
using System;
using System.Collections;

public class PointController
{
    /// <summary>
    /// Get all points on the curve
    /// </summary>
    /// <returns>The list.</returns>
    /// <param name="path">List of points that need to be passed through</param>
    /// <param name="pointSize">The number of nodes between two points</param>
    public static Vector3[] PointList(Vector3[] path, int pointSize) {
        Vector3[] controlPointList = PathControlPointGenerator(path);

        int smoothAmount = (path.Length - 1) * pointSize + 1;
        Vector3[] pointList = new Vector3[smoothAmount];

        for (int index = 0; index < smoothAmount; index++) {
            if (index % pointSize == 0) {
                pointList[index] = path[index / pointSize];
            }
            else {
                Vector3 currPt = Interp(controlPointList, (float)(index) / smoothAmount);
                pointList[index] = currPt;
            }
        }
        return pointList;
    }

    /// <summary>
    /// Get control point
    /// </summary>
    /// <returns>The control point generator.</returns>
    /// <param name="path">Path.</param>
    private static Vector3[] PathControlPointGenerator(Vector3[] path) {
        int offset = 2;
        Vector3[] suppliedPath = path;
        Vector3[] controlPoint = new Vector3[suppliedPath.Length + offset];
        Array.Copy(suppliedPath, 0, controlPoint, 1, suppliedPath.Length);

        controlPoint[0] = controlPoint[1] + (controlPoint[1] - controlPoint[2]);
        controlPoint[controlPoint.Length - 1] = controlPoint[controlPoint.Length - 2] + (controlPoint[controlPoint.Length - 2] - controlPoint[controlPoint.Length - 3]);

        if (controlPoint[1] == controlPoint[controlPoint.Length - 2]) {
            Vector3[] tmpLoopSpline = new Vector3[controlPoint.Length];
            Array.Copy(controlPoint, tmpLoopSpline, controlPoint.Length);
            tmpLoopSpline[0] = tmpLoopSpline[tmpLoopSpline.Length - 3];
            tmpLoopSpline[tmpLoopSpline.Length - 1] = tmpLoopSpline[2];
            controlPoint = new Vector3[tmpLoopSpline.Length];
            Array.Copy(tmpLoopSpline, controlPoint, tmpLoopSpline.Length);
        }

        return (controlPoint);
    }

    /// <summary>
    /// Get the point position on the curve according to T
    /// </summary>
    /// <param name="pts">Pts.</param>
    /// <param name="t">T.</param>
    private static Vector3 Interp(Vector3[] pts, float t) {
        int numSections = pts.Length - 3;
        int currPt = Mathf.Min(Mathf.FloorToInt(t * (float)numSections), numSections - 1);
        float u = t * (float)numSections - (float)currPt;

        Vector3 a = pts[currPt];
        Vector3 b = pts[currPt + 1];
        Vector3 c = pts[currPt + 2];
        Vector3 d = pts[currPt + 3];

        return .5f * (
            (-a + 3f * b - 3f * c + d) * (u * u * u)
            + (2f * a - 5f * b + 4f * c - d) * (u * u)
            + (-a + c) * u
            + 2f * b
            );
    }
}