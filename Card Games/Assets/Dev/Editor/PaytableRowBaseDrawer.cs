using UnityEditor;
using UnityEngine;

/// <summary>
/// Property Drawer for a basic Paytable Row.
/// </summary>
[CustomPropertyDrawer(typeof(PaytableRowBlackJack))]
public class PaytableRowBaseDrawer : BetterPropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddProperty("name");
        AddProperty("betMultiplier");
        AddProperty("isAWin");

        EditorGUI.EndProperty();
    }
}
