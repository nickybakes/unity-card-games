using System;
using UnityEngine;

/// <summary>
/// A display that shows the user's winnings and travels across the screen.
/// </summary>
public class WinDisplay : TravelingDisplay
{
    /// <summary>
    /// The text display that shows the winnings
    /// </summary>
    [SerializeField] private TextDisplay textDisplay;

    /// <summary>
    /// The starting transform.
    /// </summary>
    [SerializeField] private RectTransform winStartTransform;

    /// <summary>
    /// The goal transform.
    /// </summary>
    [SerializeField] private RectTransform winGoalTransform;

    /// <summary>
    /// When the destination is reached, this callback is called.
    /// </summary>
    private Action updateBetDisplayCallback;

    /// <summary>
    /// Sets up basic data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        SetupTravelingTransformData();
        StopTraveling();
    }

    /// <summary>
    /// Set the winning text and start traveling.
    /// </summary>
    /// <param name="formattedWinningsText">The currency formatted winnings text.</param>
    /// <param name="_updateBetDisplayCallback">The callback to call when done traveling.</param>
    /// <param name="isProfit">Whether this is a profit for the player or not.</param>
    public void ShowWinnings(string formattedWinningsText, Action _updateBetDisplayCallback, bool isProfit)
    {
        textDisplay.SetText("+" + formattedWinningsText, true);
        if (isProfit)
        {
            textDisplay.HighlightText();
        }
        SetStartTransform(winStartTransform);
        ApplyStartTransform();
        TravelToTransform(winGoalTransform, DoneTravelingCallback);
        updateBetDisplayCallback = _updateBetDisplayCallback;
        textDisplay.Show();
    }

    /// <summary>
    /// When the traveling is finished, invoke the callback which should update the shown balance.
    /// </summary>
    /// <param name="display"></param>
    public void DoneTravelingCallback(Display display)
    {
        textDisplay.UnhighlightText();
        textDisplay.Hide();
        updateBetDisplayCallback.Invoke();
    }

    /// <summary>
    /// Update travel data.
    /// </summary>
    public void Update()
    {
        UpdateTravel();
    }
}
