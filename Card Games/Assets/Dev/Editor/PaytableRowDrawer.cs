using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PaytableRow))]
public class PaytableRowDrawer : BetterPropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddProperty("hand");
        AddProperty("betMultiplier");

        EditorGUI.EndProperty();
    }
}
