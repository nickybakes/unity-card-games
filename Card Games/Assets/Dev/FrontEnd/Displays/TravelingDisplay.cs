using System;
using UnityEngine;

/// <summary>
/// An in-game display that travels between positions, rotations, and scales.
/// </summary>
public class TravelingDisplay : Display
{
    /// <summary>
    /// The transform data to travel from.
    /// </summary>
    private RectTransformData startTransformData;

    /// <summary>
    /// The transform data to travel towards.
    /// </summary>
    private RectTransformData goalTransformData;

    /// <summary>
    /// An animation curve that defines how the travel goes.
    /// </summary>
    [SerializeField] private AnimationCurve moveCurve;

    /// <summary>
    /// How long a travel takes, in seconds.
    /// </summary>
    [SerializeField] private float travelTimeLength = .2f;


    /// <summary>
    /// Current time, in seconds, that the display has been traveling.
    /// </summary>
    private float currentTravelTime;

    /// <summary>
    /// Callback to call when the display reaches its destination and finished traveling.
    /// </summary>
    private Action<TravelingDisplay> arrivalCallback = null;

    /// <summary>
    /// Sets up the rect transform and travel data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        SetupTravelingTransformData();
    }

    /// <summary>
    /// Sets up the travel data.
    /// </summary>
    protected void SetupTravelingTransformData()
    {
        startTransformData = new RectTransformData(rectTransform);
        goalTransformData = new RectTransformData(rectTransform);
    }

    /// <summary>
    /// Set the current traveling time to 0.
    /// </summary>
    public void StartTraveling()
    {
        currentTravelTime = 0;
    }

    /// <summary>
    /// Fully stop traveling.
    /// </summary>
    public void StopTraveling()
    {
        currentTravelTime = travelTimeLength + 1;
    }

    /// <summary>
    /// Start traveling to a given rect transform.
    /// </summary>
    /// <param name="rect">The goal rect transform.</param>
    /// <param name="_arrivalCallback">Optional callback to be called one the goal is reached.</param>
    public void TravelToTransform(RectTransform rect, Action<TravelingDisplay> _arrivalCallback = null)
    {
        SetStartTransform(rectTransform);
        SetGoalTransform(rect);
        StartTraveling();
        arrivalCallback = _arrivalCallback;
    }

    /// <summary>
    /// Start traveling to a given position, rotation, and scale.
    /// </summary>
    /// <param name="anchoredPosition">The goal position.</param>
    /// <param name="rotation">The goal rotation.</param>
    /// <param name="scale">The goal scale.</param>
    /// <param name="_arrivalCallback">Optional callback to be called one the goal is reached.</param>
    public void TravelToTransform(Vector2 anchoredPosition, Quaternion rotation, Vector3 scale, Action<TravelingDisplay> _arrivalCallback = null)
    {
        SetStartTransform(rectTransform);
        SetGoalTransform(anchoredPosition, rotation, scale);
        StartTraveling();
        arrivalCallback = _arrivalCallback;
    }

    /// <summary>
    /// Set start transform to a rect transform.
    /// </summary>
    /// <param name="rect">The rect transform to use.</param>
    public void SetStartTransform(RectTransform rect)
    {
        startTransformData.SetTransformData(rect);
    }

    /// <summary>
    /// Set goal transform to a rect transform.
    /// </summary>
    /// <param name="rect">The rect transform to use.</param>
    public void SetGoalTransform(RectTransform rect)
    {
        goalTransformData.SetTransformData(rect);
    }

    /// <summary>
    /// Set start transform to a given position, rotation, and scale.
    /// </summary>
    /// <param name="anchoredPosition">The start position.</param>
    /// <param name="rotation">The start rotation.</param>
    /// <param name="scale">The start scale.</param>
    public void SetStartTransform(Vector2 anchoredPosition, Quaternion rotation, Vector3 scale)
    {
        startTransformData.SetTransformData(anchoredPosition, rotation, scale);
    }

    /// <summary>
    /// Set goal transform to a given position, rotation, and scale.
    /// </summary>
    /// <param name="anchoredPosition">The goal position.</param>
    /// <param name="rotation">The goal rotation.</param>
    /// <param name="scale">The goal scale.</param>
    public void SetGoalTransform(Vector2 anchoredPosition, Quaternion rotation, Vector3 scale)
    {
        goalTransformData.SetTransformData(anchoredPosition, rotation, scale);
    }

    /// <summary>
    /// Apply a parent's transformations to the start transform.
    /// </summary>
    /// <param name="parent">The rect transform to use as a parent.</param>
    /// <param name="applyRotation">Whether to apply the parent's rotation.</param>
    /// <param name="applyScale">Whether to apply the parent's scale.</param>
    public void ApplyTransformParentToStartTransform(RectTransform parent, bool applyRotation = true, bool applyScale = true)
    {
        startTransformData.ApplyRectTransformParentToData(parent, applyRotation, applyScale);
    }


    /// <summary>
    /// Apply a parent's transformations to the goal transform.
    /// </summary>
    /// <param name="parent">The rect transform to use as a parent.</param>
    /// <param name="applyRotation">Whether to apply the parent's rotation.</param>
    /// <param name="applyScale">Whether to apply the parent's scale.</param>
    public void ApplyTransformParentToGoalTransform(RectTransform parent, bool applyRotation = true, bool applyScale = true)
    {
        goalTransformData.ApplyRectTransformParentToData(parent, applyRotation, applyScale);
    }

    /// <summary>
    /// Lerp between the start and goal transforms and apply it to the display.
    /// </summary>
    /// <param name="t">The lerp value.</param>
    public void ApplyLerpTransform(float t)
    {
        rectTransform.anchoredPosition = Vector2.LerpUnclamped(startTransformData.anchoredPosition, goalTransformData.anchoredPosition, t);
        rectTransform.rotation = Quaternion.LerpUnclamped(startTransformData.rotation, goalTransformData.rotation, t);
        rectTransform.localScale = Vector3.LerpUnclamped(startTransformData.scale, goalTransformData.scale, t);
    }

    /// <summary>
    /// Apply the start transform to the display.
    /// </summary>
    public void ApplyStartTransform()
    {
        startTransformData.ApplyDataToRectTranform(rectTransform);
    }

    /// <summary>
    /// Apply the goal transform to the display.
    /// </summary>
    public void ApplyGoalTransform()
    {
        goalTransformData.ApplyDataToRectTranform(rectTransform);
    }

    /// <summary>
    /// Whether this is currently traveling.
    /// </summary>
    /// <returns>Whether this is currently traveling.</returns>
    public bool isTraveling()
    {
        return currentTravelTime <= travelTimeLength;
    }

    /// <summary>
    /// Updates progress of traveling. If done traveling, applies the goal position to the display.
    /// </summary>
    public void UpdateTravel()
    {
        if (isTraveling())
        {
            currentTravelTime += Time.deltaTime;
            float lerpT = currentTravelTime / travelTimeLength;
            lerpT = moveCurve.Evaluate(lerpT);
            ApplyLerpTransform(lerpT);
            if (!isTraveling() && arrivalCallback != null)
            {
                arrivalCallback.Invoke(this);
                arrivalCallback = null;
            }
        }
        else
        {
            ApplyGoalTransform();
        }
    }
}
