using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Card))]
public class CardDrawer : BetterPropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddProperty("value");
        AddProperty("suit");
        AddProperty("special");

        EditorGUI.EndProperty();
    }
}
