using System;
using UnityEngine;

public class WinDisplay : TravelingDisplay
{

    [SerializeField] private TextDisplay textDisplay;

    [SerializeField] private RectTransform winStartTransform;
    [SerializeField] private RectTransform winGoalTransform;

    private Action updateBetDisplayCallback;

    void Awake()
    {
        SetupRectTransform();
        SetupTravelingTransformData();
        StopTraveling();
    }

    public void ShowWinnings(string formattedWinningsText, Action _updateBetDisplayCallback)
    {
        textDisplay.SetText("+" + formattedWinningsText);
        SetStartTransform(winStartTransform);
        ApplyStartTransform();
        TravelToTransform(winGoalTransform, DoneTravelingCallback);
        updateBetDisplayCallback = _updateBetDisplayCallback;
        textDisplay.Show();
    }

    public void DoneTravelingCallback(Display display)
    {
        textDisplay.Hide();
        updateBetDisplayCallback.Invoke();
    }

    public void Update()
    {
        UpdateTravel();
    }
}
