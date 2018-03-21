using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomGradient))]
public class GradientEditor : EditorWindow
{
    void OnEnable() 
    {
        titleContent.text = "Gradient Editor";
    }
}