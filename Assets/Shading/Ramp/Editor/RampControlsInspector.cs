using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RampControls))]
[CanEditMultipleObjects]
public class RampControlsInspector : Editor 
{
    RampControls _rampControls;

    void OnEnable() 
    {
        _rampControls = target as RampControls;
    }

    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        Vector3 emitterRotationEuler = EditorGUILayout.Vector3Field("Rotation Euler", _rampControls.Emitter.Rotation.eulerAngles);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_rampControls, "Emitter Rotation");
            _rampControls.Emitter.Rotation = Quaternion.Euler(emitterRotationEuler);
        }

        if (GUILayout.Button("Update shader properties"))
        {
            _rampControls.UpdateShaderProperties();
        }
    }

    void OnSceneGUI()
    {
        Transform handleTransform = _rampControls.transform;
        Vector3 emitterPosition = handleTransform.TransformPoint(_rampControls.Emitter.Position);
        
        Handles.ArrowHandleCap(10, emitterPosition, _rampControls.Emitter.Rotation, 0.5f, EventType.Repaint);

        EditorGUI.BeginChangeCheck();
        Quaternion emitterHandleRotatation = Handles.RotationHandle(_rampControls.Emitter.Rotation, emitterPosition);
        if (EditorGUI.EndChangeCheck())
        {
             Undo.RecordObject(_rampControls, "Emitter Rotation");
            _rampControls.Emitter.Rotation = emitterHandleRotatation;
        }
    }
}