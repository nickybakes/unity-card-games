using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(NoDrawRaycastTarget), false), CanEditMultipleObjects]
public class NoDrawRaycastTargetEditor : GraphicEditor
{
    public override void OnInspectorGUI()
    {
        base.serializedObject.Update();
        EditorGUILayout.PropertyField(base.m_Script, new GUILayoutOption[0]);
        base.RaycastControlsGUI();
        base.serializedObject.ApplyModifiedProperties();
    }
}