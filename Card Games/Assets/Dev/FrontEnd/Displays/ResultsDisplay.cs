using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsDisplay : MonoBehaviour
{

    [SerializeField] private GameViewManager viewManager;

    [SerializeField] private Animator animator;

    [SerializeField] private TextMeshPro text;

    [SerializeField] private List<string> genericWinStrings;
    [SerializeField] private List<string> genericLoseStrings;

    public void StartPresentation(string winText, float winMultiplier)
    {
        if (winMultiplier > 1)
        {
            if (winText == "")
                text.text = genericWinStrings[Random.Range(0, genericWinStrings.Count)];
            else
                text.text = winText;

            animator.SetTrigger("Win");
        }
        else
        {
            if (winText == "")
                text.text = genericLoseStrings[Random.Range(0, genericLoseStrings.Count)];
            else
                text.text = winText;

            // play LOSE animation
            animator.SetTrigger("Lose");
        }

    }

    public void OnPresentationFinished()
    {
        viewManager.EndResultsPresentation();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
