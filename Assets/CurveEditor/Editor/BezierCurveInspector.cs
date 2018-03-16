using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor 
{
    public const int lineSegments = 10;

    BezierCurve curve;
    Transform handleTransform;
    Quaternion handleRotation;

    void OnSceneGUI()
    {
        curve = target as BezierCurve;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;        

        Vector3 p0 = drawPointHandle(0);
        Vector3 p1 = drawPointHandle(1);
        Vector3 p2 = drawPointHandle(2);
        Vector3 p3 = drawPointHandle(3);

        drawPointsLines(p0, p1, p2, p3);
        drawBezierCurve(p0, p1, p2, p3);
    }

    void drawPointsLines(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {   
        float screenSpaceLineSize = 0.1f; 
        Handles.color = Color.gray;
        Handles.DrawDottedLine(p0, p1, screenSpaceLineSize);
        Handles.DrawDottedLine(p1, p2, screenSpaceLineSize);
        Handles.DrawDottedLine(p2, p3, screenSpaceLineSize);
    }

    void drawBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float t = 1f / lineSegments;
        Vector3 tail = BezierCubic.getPoint(p0, p1, p2, p3, 0f);
        for (int i = 1; i <= lineSegments; i++)
        {
            Handles.color = Color.red;
            Vector3 head = BezierCubic.getPoint(p0, p1, p2, p3, t * i);
            Handles.DrawLine(tail, head);
            tail = head;
        }
    }

    Vector3 getVelocity(Vector3 p0,Vector3 p1,Vector3 p2, float t)
    {
        return handleTransform.TransformPoint(BezierQuadratic.getTrangent(p0, p1, p2, t) - p0);
    }

    void drawVelocityDirection(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {

    }

    Vector3 drawPointHandle(int index)
    {
        Vector3 point = handleTransform.TransformPoint(curve.points[index]);
        EditorGUI.BeginChangeCheck();
            point = Handles.PositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(point);

        return point;
    }
}
