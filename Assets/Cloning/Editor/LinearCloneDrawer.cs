using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

[CustomPropertyDrawer(typeof(LinearClone))]
public class LinearCloneDrawer: PropertyDrawer 
{
    float _rowHeight = 15;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
    {
        EditorGUI.BeginProperty(position, label, property);

        object obj = property.serializedObject.targetObject;
        LinearClone LP = obj.GetType().GetField(property.name).GetValue(obj) as LinearClone;

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect LayoutRect = new Rect(position.x, position.y, position.width, _rowHeight);
        
        var countProperty = typeof(LinearClone).GetProperty("Count");
        countProperty.SetValue(LP, EditorGUI.IntField(LayoutRect, countProperty.Name, (int)countProperty.GetValue(LP, null)), null);

        EditorGUI.EndProperty();
    }
}