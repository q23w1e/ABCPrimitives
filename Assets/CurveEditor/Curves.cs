using UnityEngine;

public static class BezierQuadratic
{
    public static Vector3 getPoint(Vector3 p0,Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        Vector3 p01 = Vector3.Lerp(p0, p1, t);
        Vector3 p12 = Vector3.Lerp(p1, p2, t);

        // return Vector3.Lerp(p01, p12, t);
        return (1f - t) * (1f - t) * p0 + 2 * t * (1f - t) * p1 + t * t * p2;
        // return Vector3.Lerp(p01, p12, t);
    }

    public static Vector3 getTrangent(Vector3 p0,Vector3 p1, Vector3 p2, float t)
    {
        // return (1 - t) * p0 + 2 * (1 - t) * p1 + p1 + p2;
        return 2f * (1f - t) * (p1 - p0) + 2f * t * (p2 - p1);
    }
}

public static class BezierCubic
{
    public static Vector3 getPoint(Vector3 p0,Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 p01 = Vector3.Lerp(p0, p1, t);
        Vector3 p12 = Vector3.Lerp(p1, p2, t);
        Vector3 p23 = Vector3.Lerp(p2, p3, t);

        return Vector3.Lerp(Vector3.Lerp(p01, p12, t), Vector3.Lerp(p12, p23, t), t);
    }

    public static Vector3 getFirstDerivative(Vector3 p0,Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 3 * (1f - t) * (1f - t) * (p1 - p0) + 6 * (1f - t) * t * (p2 - p1) + 3 * t * t * (p3 - p2);
    }

    public static Vector3 getSecondDerivative(Vector3 p0,Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 6 * (1f - t) * (p2 - 2* p1 + p0) + 6 * t * (p3 - 2* p2 + p1);
    }
}

public static class DeCasteljauQubic
{
    // I need a smarter way to initialize the array of arbitrary n size
    static Vector3[][] _joints = new Vector3[4][]
    {
        new Vector3[4],
        new Vector3[3],
        new Vector3[2],
        new Vector3[1]
    };

    public static Vector3 getPoint(Vector3[] points, float t)
    {
        int n = points.Length;
        _joints[0] = points;
        
        for (int k = 1; k < n; k++)
            for (int i = 0; i < n - k; i++)
                _joints[k][i] = (1 - t) * _joints[k - 1][i] + t * _joints[k - 1][i + 1];

        return _joints[n - 1][0];
    }
}
