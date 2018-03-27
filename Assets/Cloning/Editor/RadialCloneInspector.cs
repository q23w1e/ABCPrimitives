using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RadialClone))]
class RadialCloneInspector: Editor
{
    RadialClone cloner;

    void OnEnable() 
    {
        cloner = target as RadialClone;
        Tools.hidden = true;
    }

    void OnDisable() 
    {
        Tools.hidden = false;
    }

    public override void OnInspectorGUI()
    {
        cloner.prefab = EditorGUILayout.ObjectField("Prefab", cloner.prefab, typeof(GameObject), true) as GameObject;
        cloner.Count = EditorGUILayout.IntField("Count", cloner.Count);
        cloner.Radius =  EditorGUILayout.FloatField("Radius", cloner.Radius);

        if (GUILayout.Button(" Generate"))
        {
            cloner.updateClonesStack();
            cloner.recalculatePositions();
        }

        if (GUILayout.Button("Clear Stack")) 
        { 
            cloner.Clear();
        }
    }

    void OnSceneGUI()
    {
        if (cloner.containerTransform.hasChanged)
        {
            cloner.recalculatePositions();
        }
    }
}