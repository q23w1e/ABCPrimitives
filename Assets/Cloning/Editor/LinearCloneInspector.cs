using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LinearClone))]
class LinearCloneInspector: Editor
{
    Transform handleTransform;
    LinearClone cloner;

    void OnEnable() 
    {
        cloner = target as LinearClone;
        handleTransform = cloner.transform;
    }

    public override void OnInspectorGUI()
    {
        cloner.prefab = EditorGUILayout.ObjectField("Prefab", cloner.prefab, typeof(GameObject), true) as GameObject;
        cloner.Count = EditorGUILayout.IntField("Count", cloner.Count);
        cloner.Head = EditorGUILayout.Vector3Field("End Point", cloner.Head);

        if (GUILayout.Button("Clone!"))
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
        
        drawHeadPositionHandle();
        drawClonePath();

        if (handleTransform.hasChanged)
        {
            cloner.recalculatePositions();
        }
    }

    void drawClonePath()
    {
        Handles.color = Color.green;
        Handles.DrawDottedLine(cloner.Tail, cloner.Head, 0.2f);
    }

    void drawHeadPositionHandle()
    {
        Vector3 head = cloner.Head;
        EditorGUI.BeginChangeCheck();
            head = Handles.PositionHandle(head, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cloner, "Move Point");
            EditorUtility.SetDirty(cloner);
            cloner.Head = head;
            cloner.recalculatePositions();
        }
    }
}