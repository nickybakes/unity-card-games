using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A layout group of text objects intended for the paytable.
/// </summary>
public class PaytableTextGroup : MonoBehaviour
{
    /// <summary>
    /// The TExt Display prefab to spawn.
    /// </summary>
    [SerializeField] private TextDisplay prefab;

    /// <summary>
    /// Reference to spawned text prefabs
    /// </summary>
    private List<TextDisplay> textDisplays;

    /// <summary>
    /// Initialize.
    /// </summary>
    void Awake()
    {
        textDisplays = new List<TextDisplay>();
    }

    /// <summary>
    /// Add a new text display to this as a child.
    /// </summary>
    /// <param name="text">The text to show on this display.</param>
    public void AddTextDisplay(string text)
    {
        TextDisplay newTextDisplay = Instantiate(prefab, transform).GetComponent<TextDisplay>();
        newTextDisplay.SetText(text);
        textDisplays.Add(newTextDisplay);
    }

    /// <summary>
    /// Highlight a text display at an index.
    /// </summary>
    /// <param name="index">The index of the text display to highlight.</param>
    public void HighlightText(int index)
    {
        textDisplays[index].HighlightText();
    }

    /// <summary>
    /// Unighlight a text display at an index.
    /// </summary>
    /// <param name="index">The index of the text display to unhighlight.</param>
    public void UnhighlightText(int index)
    {
        textDisplays[index].UnhighlightText();
    }
}
