using TMPro;
using UnityEngine;

/// <summary>
/// An in game display that shows text and can be hidden, highlighted, etc.
/// </summary>
public class TextDisplay : Display
{
    /// <summary>
    /// The text element to use.
    /// </summary>
    [SerializeField] protected TextMeshProUGUI textLabel;

    /// <summary>
    /// The animator that controls this text display.
    /// </summary>
    [SerializeField] protected Animator animator;

    /// <summary>
    /// The text material to put on the text when highlighted.
    /// </summary>
    [SerializeField] protected Material highlightMaterial;

    /// <summary>
    /// The original material of the text when awoken.
    /// </summary>
    private Material normalMaterial;

    /// <summary>
    /// Sets up display rect and original text material.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        normalMaterial = textLabel.fontMaterial;
    }

    /// <summary>
    /// Plays the hide animation.
    /// </summary>
    public virtual void Hide()
    {
        if (animator != null)
            animator.SetTrigger("Hide");
    }

    /// <summary>
    /// Plays the show animation.
    /// </summary>
    public virtual void Show()
    {
        if (animator != null)
            animator.SetTrigger("Show");
    }

    /// <summary>
    /// Set the text and if its different, play a bump animation.
    /// </summary>
    /// <param name="text">The text string to show.</param>
    /// <param name="forceBumpAnimation">Optional bool of whether to force the bump animation to play no matter what.</param>
    public virtual void SetText(string text, bool forceBumpAnimation = false)
    {
        if (animator != null && (textLabel.text != text || forceBumpAnimation))
            animator.SetTrigger("Bump");

        textLabel.text = text;
    }

    /// <summary>
    /// Set the text's material to the highlight material.
    /// </summary>
    public virtual void HighlightText()
    {
        textLabel.fontMaterial = highlightMaterial;
    }

    /// <summary>
    /// Set the text's material to the original material.
    /// </summary>
    public virtual void UnhighlightText()
    {
        textLabel.fontMaterial = normalMaterial;
    }
}
