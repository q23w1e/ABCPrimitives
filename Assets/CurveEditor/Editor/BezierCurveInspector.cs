using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor 
{

    BezierCurve curve;
    Transform handleTransform;
    Quaternion handleRotation;

    void OnSceneGUI()
    {
        curve = target as BezierCurve;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;        

        drawPointHandle(0);
        drawPointHandle(1);
        drawPointHandle(2);

        drawPointsLines();
    }
    
    void drawPointsLines()
    {
        Handles.color = Color.gray;
        int[] segmentIndices = new int[] {0, 1, 1, 2};
        Vector3[] transformedPoints = new Vector3[curve.points.Length];
        
        Handles.DrawDottedLines(curve.points, segmentIndices, 0.5f);
    }

    void drawPointHandle(int index)
    {
        Vector3 p = handleTransform.TransformPoint(curve.points[index]);
        EditorGUI.BeginChangeCheck();
            p = Handles.PositionHandle(p, handleRotation);
        if (EditorGUI.EndChangeCheck())
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(p);
        // drawPointsLines();
    }
}
