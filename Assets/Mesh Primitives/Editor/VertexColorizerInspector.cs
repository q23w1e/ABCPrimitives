using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VertexColorizer))]
public class VertexColorizerInspector : Editor 
{
    VertexColorizer _vertexColorizer;

    void OnEnable() 
    {
        _vertexColorizer = target as VertexColorizer;
    }

    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        Color pickedColor = EditorGUILayout.ColorField(_vertexColorizer.Color);
        if (EditorGUI.EndChangeCheck())
        {
            _vertexColorizer.Color = pickedColor;
        }
    }
}