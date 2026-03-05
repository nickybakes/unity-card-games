using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The Panel that handles showing the round result and doing the associated celebration animations.
/// </summary>
public class ResultPanel : MonoBehaviour
{

    /// <summary>
    /// Reference to the Game View Manager.
    /// </summary>
    [SerializeField] private GameViewManager viewManager;

    /// <summary>
    /// The animator that controls the Result Panel.
    /// </summary>
    [SerializeField] private Animator animator;

    /// <summary>
    /// Text objects in the Result Panel to show the round result string.
    /// </summary>
    [SerializeField] private List<TextMeshProUGUI> texts;

    /// <summary>
    /// If the Paytable/Game dont provide a Winning round result string, pick from a defined set of generic ones.
    /// </summary>
    [SerializeField] private List<string> genericWinStrings;

    /// <summary>
    /// If the Paytable/Game dont provide a Losing round result string, pick from a defined set of generic ones.
    /// </summary>
    [SerializeField] private List<string> genericLoseStrings;

    /// <summary>
    /// Sets the text and plays the celebration animation.
    /// </summary>
    /// <param name="resultText">The round result string to show. Use empty string to have a generic string picked.</param>
    /// <param name="isAWin">Whether this result is considered a win or not.</param>
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

    /// <summary>
    /// Sets all the text objects to show a specific text string.
    /// </summary>
    /// <param name="text">The string to show.</param>
    private void SetText(string text)
    {
        foreach (TextMeshProUGUI textObject in texts)
        {
            textObject.text = text;
        }
    }

    /// <summary>
    /// When the presentation is done, tell the view manager its done.
    /// </summary>
    public void OnPresentationFinished()
    {
        viewManager.EndResultPresentation();
    }
}
