using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Line))]
public class LineEditor : Editor 
{

	void OnSceneGUI()
	{
		Line line = target as Line;
		Transform handleTranform = line.transform;
		Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
		handleTranform.rotation : Quaternion.identity;

		Vector3 p0 = handleTranform.TransformPoint(line.p0);
		Vector3 p1 = handleTranform.TransformPoint(line.p1);

		Handles.color = Color.white;
		Handles.DrawLine(p0, p1);

		EditorGUI.BeginChangeCheck();
		p0 = Handles.PositionHandle(p0, handleRotation);
			Undo.RecordObject(line, "Move Point");
			EditorUtility.SetDirty(line);
			if (EditorGUI.EndChangeCheck())
				line.p0 = handleTranform.InverseTransformPoint(p0);
		EditorGUI.BeginChangeCheck();
		p1 = Handles.PositionHandle(p1, handleRotation);
			Undo.RecordObject(line, "Move Point");
			EditorUtility.SetDirty(line);
			if (EditorGUI.EndChangeCheck())
				line.p1 = handleTranform.InverseTransformPoint(p1);
	}
}
