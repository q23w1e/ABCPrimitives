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
        startPositionAsTransform = cloner.transform;
    }

    public override void OnInspectorGUI()
    {
        cloner.prefab = EditorGUILayout.ObjectField("Prefab", cloner.prefab, typeof(GameObject), true) as GameObject;
        cloner.Count = EditorGUILayout.IntField("Count", cloner.Count);
        cloner.EndPosition = EditorGUILayout.Vector3Field("End Position", cloner.EndPosition);

        if (GUILayout.Button("Generate"))
        {
            // I need to find a way to call updateClonesList() automatically inside Count property in order to get rid of this useless button 
            cloner.updateClonesList();
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
        
        drawEndPositionHandle();
        drawClonePath();

        if (startPositionAsTransform.hasChanged)
        {
            cloner.recalculatePositions();
        }
    }

    void drawClonePath()
    {
        Handles.color = Color.green;
        Handles.DrawDottedLine(cloner.StartPosition, cloner.EndPosition, 0.2f);
    }

    void drawEndPositionHandle()
    {
        EditorGUI.BeginChangeCheck();
            Vector3 endPosition = Handles.PositionHandle(cloner.EndPosition, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cloner, "Move End Position");
            EditorUtility.SetDirty(cloner);
            
            cloner.EndPosition = endPosition;
            cloner.recalculatePositions();
        }
    }
}