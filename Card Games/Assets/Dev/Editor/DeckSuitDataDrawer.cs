using UnityEditor;
using UnityEngine;

/// <summary>
/// Property Drawer for Deck Suit Data.
/// </summary>
[CustomPropertyDrawer(typeof(DeckSuitData))]
public class DeckSuitDataDrawer : BetterPropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddProperty("suit");

        AddQuarterBlankLine();

        SerializedProperty allowedValuesProperty = property.FindPropertyRelative("values");

        if (allowedValuesProperty.arraySize == 0)
        {
            for (int i = 0; i < 13; i++)
            {
                allowedValuesProperty.InsertArrayElementAtIndex(0);
                allowedValuesProperty.GetArrayElementAtIndex(0).boolValue = true;
            }
        }

        StartSameLine(allowedValuesProperty.arraySize);

        for (int i = 0; i < allowedValuesProperty.arraySize; i++)
        {
            AddLabel(Card.CARD_VALUE_STRINGS[i]);
        }

        StartSameLine(allowedValuesProperty.arraySize);

        for (int i = 0; i < allowedValuesProperty.arraySize; i++)
        {
            AddProperty("", "", allowedValuesProperty.GetArrayElementAtIndex(i));
        }

        AddQuarterBlankLine();

        EditorGUI.EndProperty();
    }
}
