using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{

    [SerializeField] private GameViewManager viewManager;

    [SerializeField] private Animator animator;

    [SerializeField] private List<TextMeshProUGUI> texts;

    [SerializeField] private List<string> genericWinStrings;
    [SerializeField] private List<string> genericLoseStrings;

    public void StartPresentation(string resultText, bool isAWin)
    {
        if (isAWin)
        {
            if (resultText == "")
                SetText(genericWinStrings[Random.Range(0, genericWinStrings.Count)]);
            else
                SetText(resultText);

            animator.SetTrigger("Win");
        }
        else
        {
            if (resultText == "")
                SetText(genericLoseStrings[Random.Range(0, genericLoseStrings.Count)]);
            else
                SetText(resultText);

            animator.SetTrigger("Lose");
        }

    }

    private void SetText(string text)
    {
        foreach (TextMeshProUGUI textObject in texts)
        {
            textObject.text = text;
        }
    }

    public void OnPresentationFinished()
    {
        viewManager.EndResultPresentation();
    }
}
