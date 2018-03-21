using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CustomGradient))]
public class GradientDrawer: PropertyDrawer 
{
    int _textureWidth = 128;

    // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    // {
    //     return _propertyHeight;
    // }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
    {
        Event guiEvent = Event.current;
        CustomGradient gradient = fieldInfo.GetValue(property.serializedObject.targetObject) as CustomGradient;
        float labelWidth = GUI.skin.label.CalcSize(label).x + 5;
        Rect textureRect = new Rect(position.x + labelWidth, position.y, position.width - labelWidth, position.height);

        if (guiEvent.type == EventType.Repaint)
        {
            GUI.Label(position, label);

            GUIStyle gradientStyle = new GUIStyle();
            gradientStyle.normal.background = gradient.GetTexture(_textureWidth);
            GUI.Label(textureRect, GUIContent.none, gradientStyle);
        }
        else 
        {
            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
            {
                if (textureRect.Contains(guiEvent.mousePosition))
                {
                    EditorWindow.GetWindow<GradientEditor>();
                }
            }
        }

    }
}