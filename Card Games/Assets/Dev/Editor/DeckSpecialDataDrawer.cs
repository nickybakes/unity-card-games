using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DeckSpecialData))]
public class DeckSpecialDataDrawer : BetterPropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddProperty("type");
        AddProperty("amount");

        EditorGUI.EndProperty();
    }
}
