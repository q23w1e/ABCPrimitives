using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RampUnlitControls))]
[CanEditMultipleObjects]
public class RampUnlitControlsInspector : Editor 
{
    RampUnlitControls _controls;

    void OnEnable()
    {
        _controls = target as RampUnlitControls;
    }

    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        Vector3 emitterRotationEuler = EditorGUILayout.Vector3Field("Rotation Euler", _controls.Emitter.Rotation.eulerAngles);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_controls, "Emitter Rotation");
            EditorUtility.SetDirty(_controls);
            _controls.Emitter.Rotation = Quaternion.Euler(emitterRotationEuler);
        }

        if (GUILayout.Button("Update shader properties"))
        {
            _controls.UpdateShaderProperties();
        }
    }
}