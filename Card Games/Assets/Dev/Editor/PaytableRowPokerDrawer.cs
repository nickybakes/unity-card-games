using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PaytableRowPoker))]
public class PaytableRowPokerDrawer : BetterPropertyDrawer
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
