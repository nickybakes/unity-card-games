using TMPro;
using UnityEngine;

public class TextDisplay : Display
{
    [SerializeField] protected TextMeshProUGUI textLabel;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Material highlightMaterial;

    private Material normalMaterial;

    void Awake()
    {
        SetupRectTransform();
        normalMaterial = textLabel.fontMaterial;
    }

    public virtual void Hide()
    {
        if (animator != null)
            animator.SetTrigger("Hide");
    }

    public virtual void Show()
    {
        if (animator != null)
            animator.SetTrigger("Show");
    }

    public virtual void SetText(string text, bool forceBumpAnimation = false)
    {
        if (animator != null && (textLabel.text != text || forceBumpAnimation))
            animator.SetTrigger("Bump");

        textLabel.text = text;
    }

    public virtual void HighlightText()
    {
        textLabel.fontMaterial = highlightMaterial;
    }

    public virtual void UnhighlightText()
    {
        textLabel.fontMaterial = normalMaterial;
    }
}
