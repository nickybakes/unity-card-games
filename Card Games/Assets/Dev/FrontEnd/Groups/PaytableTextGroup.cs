using System.Collections.Generic;
using UnityEngine;

public class PaytableTextGroup : MonoBehaviour
{
    [SerializeField] private TextDisplay prefab;

    private List<TextDisplay> textDisplays;

    void Awake()
    {
        textDisplays = new List<TextDisplay>();
    }

    public void AddTextDisplay(string text)
    {
        TextDisplay newTextDisplay = Instantiate(prefab, transform).GetComponent<TextDisplay>();
        newTextDisplay.SetText(text);
        textDisplays.Add(newTextDisplay);
    }

    public void HighlightText(int index)
    {
        textDisplays[index].HighlightText();
    }

    public void UnhighlightText(int index)
    {
        textDisplays[index].UnhighlightText();
    }
}
