using TMPro;
using UnityEngine;

public class TextDisplay : Display
{
    [SerializeField] private TextMeshProUGUI textLabel;
    [SerializeField] private Animator animator;

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    public void SetText(string text)
    {
        if (textLabel.text != text)
            animator.SetTrigger("Bump");

        textLabel.text = text;
    }
}
