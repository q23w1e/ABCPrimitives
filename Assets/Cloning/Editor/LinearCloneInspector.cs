using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LinearClone))]
class LinearCloneInspector: Editor
{
    Transform startPositionAsTransform;
    LinearClone cloner;

    void OnEnable() 
    {
        cloner = target as LinearClone;
        Tools.hidden = true;
    }

    void OnDisable() 
    {
        Tools.hidden = false;
    }

    public override void OnInspectorGUI()
    {
        cloner.prefab = EditorGUILayout.ObjectField("Prefab", cloner.prefab, typeof(GameObject), true) as GameObject;
        cloner.Count = EditorGUILayout.IntField("Count", cloner.Count);
        cloner.StartPosition = EditorGUILayout.Vector3Field("End Position", cloner.StartPosition);
        cloner.EndPosition = EditorGUILayout.Vector3Field("End Position", cloner.EndPosition);

        if (GUILayout.Button("Generate"))
        {
            cloner.updateClonesStack();
            cloner.recalculatePositions();
        }
        if (GUILayout.Button("Clear Stack"))
        {
            cloner.Clear();
        }

    }

    void OnSceneGUI()
    {
        cloner = target as LinearClone;
        
        cloner.start = showPositionHPoint(cloner.start);
        cloner.end = showPositionHPoint(cloner.end);
        cloner.recalculatePositions();
        drawClonePath();

        if (cloner.containerTransform.hasChanged)
        {
            drawClonePath();
            cloner.recalculatePositions();
        }
    }

    void updateHandles()
    {
        cloner.start = showPositionHPoint(cloner.start);
        cloner.end = showPositionHPoint(cloner.end);
        drawClonePath();
        cloner.recalculatePositions();
    }

    void drawClonePath()
    {
        Handles.color = Color.green;
        Handles.DrawDottedLine(cloner.StartPosition, cloner.EndPosition, 0.2f);
    }

    Vector3 showPositionHPoint(Vector3 point)
    {
        Vector3 endPosition = cloner.containerTransform.TransformPoint(point);
        EditorGUI.BeginChangeCheck();
            endPosition = Handles.DoPositionHandle(endPosition, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cloner, "Move Linear Clone Position");
            EditorUtility.SetDirty(cloner);
            
            point = cloner.containerTransform.InverseTransformPoint(endPosition);
        }

        return point;
    }
}