using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PokerHandRule))]
public class PokerHandRuleDrawer : BetterPropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        AddProperty("type");

        AddQuarterBlankLine();

        StartSameLine(2);

        AddProperty("amountNeeded");
        AddProperty("exactAmount");

        AddQuarterBlankLine();

        AddLabel("Allowed Values");
        StartSameLine(26);
        SerializedProperty allowedValuesProperty = property.FindPropertyRelative("allowedValues");

        if (allowedValuesProperty.arraySize == 0)
        {
            for (int i = 0; i < 13; i++)
            {
                allowedValuesProperty.InsertArrayElementAtIndex(0);
                allowedValuesProperty.GetArrayElementAtIndex(0).boolValue = true;
            }
        }

        for (int i = 0; i < allowedValuesProperty.arraySize; i++)
        {
            AddLabel(Card.CARD_VALUE_STRINGS[i]);
            AddProperty("", "", allowedValuesProperty.GetArrayElementAtIndex(i));
        }

        AddQuarterBlankLine();

        AddLabel("Allowed Suits");

        StartSameLine(8);

        SerializedProperty allowedSuitsProperty = property.FindPropertyRelative("allowedSuits");

        if (allowedSuitsProperty.arraySize == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                allowedSuitsProperty.InsertArrayElementAtIndex(0);
                allowedSuitsProperty.GetArrayElementAtIndex(0).boolValue = true;
            }
        }

        for (int i = 0; i < allowedSuitsProperty.arraySize; i++)
        {
            AddLabel(Card.CARD_SUIT_STRINGS[i]);
            AddProperty("", "", allowedSuitsProperty.GetArrayElementAtIndex(i));
        }

        AddQuarterBlankLine();

        EditorGUI.EndProperty();
    }

}
