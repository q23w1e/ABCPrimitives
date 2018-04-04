using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralGrid))]
public class ProceduralGridInspector : Editor 
{
    ProceduralGrid _grid;

    void OnEnable() 
    {
        _grid = target as ProceduralGrid;
    }

    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();    

        EditorGUI.BeginChangeCheck();
        int width = EditorGUILayout.IntField("Width", _grid.Width);
        if (EditorGUI.EndChangeCheck())
        {
            _grid.Width = width;
        }

        EditorGUI.BeginChangeCheck();
        int height = EditorGUILayout.IntField("Height", _grid.Height);
        if (EditorGUI.EndChangeCheck())
        {
            _grid.Height = height;
        }

        if (GUILayout.Button("Generate"))
        {
            _grid.Generate();
        }
    }
}