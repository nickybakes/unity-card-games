using TMPro;
using UnityEngine;

public class TextDisplay : Display
{
    [SerializeField] protected TextMeshProUGUI textLabel;
    [SerializeField] protected Animator animator;

    public virtual void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public virtual void Show()
    {
        animator.SetTrigger("Show");
    }

    public virtual void SetText(string text, bool forceBumpAnimation = false)
    {
        if (textLabel.text != text || forceBumpAnimation)
            animator.SetTrigger("Bump");

        textLabel.text = text;
    }

    public virtual void HighlightText()
    {
        animator.SetTrigger("Highlight");
    }

    public virtual void UnhighlightText()
    {
        animator.SetTrigger("Unhighlight");
    }
}
