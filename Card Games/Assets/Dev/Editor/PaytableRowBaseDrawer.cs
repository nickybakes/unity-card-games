using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PaytableRowBlackJack))]
public class PaytableRowBaseDrawer : BetterPropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddProperty("name");
        AddProperty("betMultiplier");

        EditorGUI.EndProperty();
    }
}
