using UnityEditor;
using UnityEngine;

/// <summary>
/// Property Drawer extension that keeps track of its position in the Inspector.
/// </summary>
public class BetterPropertyDrawer : PropertyDrawer
{

    /// <summary>
    /// Cached position rect data.
    /// </summary>
    protected Rect position;

    /// <summary>
    /// The current property being drawn.
    /// </summary>
    protected SerializedProperty property;

    /// <summary>
    /// The current height of the property including children.
    /// </summary>
    protected float childrenHeight;

    /// <summary>
    /// The current line height being drawn to. 
    /// </summary>
    protected float currentSameLineHeight;

    /// <summary>
    /// An override for the width of a property.
    /// </summary>
    protected float normalizedWidthOverride = -1;

    /// <summary>
    /// An override for where horizontally on the current line to draw.
    /// </summary>
    protected float normalizedXPositionOverride = -1;

    /// <summary>
    /// The index along the current line.
    /// </summary>
    protected int sameLineCurrentIndex = 0;

    /// <summary>
    /// The amount of elements to fit on one line.
    /// </summary>
    protected int sameLineAmount = 1;

    /// <summary>
    /// Stores the given properties and resets the children height value.
    /// </summary>
    /// <param name="position">The position Rect.</param>
    /// <param name="property">The serialized property.</param>
    /// <param name="label">The content label.</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        childrenHeight = 0;
        this.position = position;
        this.property = property;
    }

    /// <summary>
    /// Adds a serialized property to the inspector.
    /// </summary>
    /// <param name="propertyName">The string name needed to find the serialized property.</param>
    /// <param name="displayName">A different name to display.</param>
    /// <param name="serializedPropertyOverride">Instead of finding a property, use this to override with a serialized property.</param>
    /// <returns>The serialized property found with the specific string name.</returns>
    protected SerializedProperty AddProperty(string propertyName, string displayName = null, SerializedProperty serializedPropertyOverride = null)
    {
        SerializedProperty thisProperty = serializedPropertyOverride;

        if (thisProperty == null)
        {
            thisProperty = property.FindPropertyRelative(propertyName);
        }

        if (displayName != null)
        {
            GUIContent label = new GUIContent(displayName);
            EditorGUI.PropertyField(Position(), thisProperty, label);
        }
        else
        {
            EditorGUI.PropertyField(Position(), thisProperty);
        }

        NextLine(EditorGUI.GetPropertyHeight(thisProperty, true));

        return thisProperty;
    }

    /// <summary>
    /// Adds a Header Foldout element to the inspector.
    /// </summary>
    /// <param name="text">The text to show on the label.</param>
    /// <param name="foldout">Whether the foldout is opened or not.</param>
    /// <returns>Returns if the foldout is opened or not.</returns>
    protected bool AddHeaderFoldout(string text, bool foldout)
    {
        Rect position = Position();
        NextLine();
        return EditorGUI.Foldout(position, foldout, text, EditorStyles.foldoutHeader);
    }

    /// <summary>
    /// Adds a Foldout element to the inspector.
    /// </summary>
    /// <param name="text">The text to show on the label.</param>
    /// <param name="foldout">Whether the foldout is opened or not.</param>
    /// <returns>Returns if the foldout is opened or not.</returns>
    protected bool AddFoldout(string text, bool foldout)
    {
        Rect position = Position();
        NextLine();
        return EditorGUI.Foldout(position, foldout, text);
    }


    /// <summary>
    /// Adds a Label element to the inspector.
    /// </summary>
    /// <param name="text">The text to show on the label.</param>
    protected void AddLabel(string text)
    {
        Rect position = Position();
        NextLine();
        EditorGUI.LabelField(position, text);
    }

    /// <summary>
    /// Adds an integer slider to the inspector.
    /// </summary>
    /// <param name="value">The value to show on the slider.</param>
    /// <param name="leftValue">The left bound value.</param>
    /// <param name="rightValue">The right bound value.</param>
    /// <returns>The value the slider is now at.</returns>
    protected int AddIntSlider(int value, int leftValue, int rightValue)
    {
        Rect position = Position();
        NextLine();
        return EditorGUI.IntSlider(position, value, leftValue, rightValue);
    }

    /// <summary>
    /// Add an integer input field to the inspector.
    /// </summary>
    /// <param name="value">The value to show on the field.</param>
    /// <returns>The value the field is now at.</returns>
    protected int AddIntField(int value)
    {
        Rect position = Position();
        NextLine();
        return EditorGUI.IntField(position, value);
    }

    /// <summary>
    /// Add a checkbox toggle to the inspector.
    /// </summary>
    /// <param name="value">The value to show on the checkbox.</param>
    /// <returns>The value the checkbox is now at.</returns>
    protected bool AddCheckbox(bool value)
    {
        Rect position = Position();
        NextLine();
        return EditorGUI.Toggle(position, value);
    }

    /// <summary>
    /// Add a button to the inspector.
    /// </summary>
    /// <param name="text">The text to show in the button.</param>
    /// <returns>Whether the button has been clicked.</returns>
    protected bool Button(string text)
    {
        Rect position = Position();
        NextLine();
        return GUI.Button(position, text);
    }

    /// <summary>
    /// Move onto the next line in the inspector.
    /// </summary>
    /// <param name="newHeight">The height of the current line. Use -1 for default single line height.</param>
    private void NextLine(float newHeight = -1)
    {
        if (newHeight == -1)
            newHeight = EditorGUIUtility.singleLineHeight;

        if (newHeight > currentSameLineHeight)
            currentSameLineHeight = newHeight;

        if (sameLineCurrentIndex == sameLineAmount - 1)
        {
            childrenHeight += currentSameLineHeight;
            currentSameLineHeight = 0;
            sameLineCurrentIndex = 0;
            sameLineAmount = 1;
        }
        else
        {
            sameLineCurrentIndex++;
        }
    }

    /// <summary>
    /// Add a blank line to the inspector.
    /// </summary>
    protected void AddBlankLine()
    {
        childrenHeight += EditorGUIUtility.singleLineHeight;
    }

    /// <summary>
    /// Add half of a blank line to the inspector.
    /// </summary>
    protected void AddHalfBlankLine()
    {
        childrenHeight += EditorGUIUtility.singleLineHeight / 2f;
    }

    /// <summary>
    /// Add quarter of a blank line to the inspector.
    /// </summary>
    protected void AddQuarterBlankLine()
    {
        childrenHeight += EditorGUIUtility.singleLineHeight / 4f;
    }

    /// <summary>
    /// Get the height of a property area.
    /// </summary>
    /// <param name="property">The property to get the height of.</param>
    /// <param name="label">Descriptive text or image.</param>
    /// <returns>The height of the property.</returns>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return childrenHeight;
    }

    /// <summary>
    /// Start putting properties on the same line.
    /// </summary>
    /// <param name="amount">The number of elements to put on the same line.</param>
    protected void StartSameLine(int amount)
    {
        sameLineCurrentIndex = 0;
        sameLineAmount = amount;
    }

    /// <summary>
    /// Skips a slot on the same line.
    /// </summary>
    protected void SkipSameLineSlot()
    {
        NextLine(0);
    }

    /// <summary>
    /// Calculate a rect for positioning a property in the inspector.
    /// </summary>
    /// <returns>The position rect.</returns>
    protected Rect Position()
    {
        if (normalizedWidthOverride != -1 && normalizedXPositionOverride != -1)
        {
            return new Rect(position.x + (position.width * normalizedXPositionOverride), position.y + childrenHeight, position.width * normalizedWidthOverride, EditorGUIUtility.singleLineHeight);
        }
        float width = position.width / sameLineAmount;
        return new Rect(position.x + (width * sameLineCurrentIndex), position.y + childrenHeight, width, EditorGUIUtility.singleLineHeight);
    }
}
