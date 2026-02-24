using System;
using UnityEngine;

public class TravelingDisplay : Display
{

    private RectTransformData startTransformData;

    private RectTransformData goalTransformData;

    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private float travelTimeLength = .2f;

    private float currentTravelTime;

    private Action<TravelingDisplay> arrivalCallback = null;

    void Awake()
    {
        SetupRectTransform();
        SetupTravelingTransformData();
    }

    protected void SetupTravelingTransformData()
    {
        startTransformData = new RectTransformData(rectTransform);
        goalTransformData = new RectTransformData(rectTransform);
    }

    public void StartTraveling()
    {
        currentTravelTime = 0;
    }

    public void StopTraveling()
    {
        currentTravelTime = travelTimeLength + 1;
    }

    public void TravelToTransform(RectTransform rect, Action<TravelingDisplay> _arrivalCallback = null)
    {
        SetStartTransform(rectTransform);
        SetGoalTransform(rect);
        StartTraveling();
        arrivalCallback = _arrivalCallback;
    }

    public void SetStartTransform(RectTransform rect)
    {
        startTransformData.SetTransformData(rect);
    }

    public void SetGoalTransform(RectTransform rect)
    {
        goalTransformData.SetTransformData(rect);
    }

    public void SetStartTransform(Vector2 anchoredPosition, Quaternion rotation, Vector3 scale)
    {
        startTransformData.SetTransformData(anchoredPosition, rotation, scale);
    }

    public void SetGoalTransform(Vector2 anchoredPosition, Quaternion rotation, Vector3 scale)
    {
        goalTransformData.SetTransformData(anchoredPosition, rotation, scale);
    }

    public void ApplyTransformParentToStartTransform(RectTransform parent, bool applyRotation = true, bool applyScale = true)
    {
        startTransformData.ApplyRectTransformParentToData(parent, applyRotation, applyScale);
    }

    public void ApplyTransformParentToGoalTransform(RectTransform parent, bool applyRotation = true, bool applyScale = true)
    {
        goalTransformData.ApplyRectTransformParentToData(parent, applyRotation, applyScale);
    }

    public void ApplyLerpTransform(float t)
    {
        rectTransform.anchoredPosition = Vector2.LerpUnclamped(startTransformData.anchoredPosition, goalTransformData.anchoredPosition, t);
        rectTransform.rotation = Quaternion.LerpUnclamped(startTransformData.rotation, goalTransformData.rotation, t);
        rectTransform.localScale = Vector3.LerpUnclamped(startTransformData.scale, goalTransformData.scale, t);
    }

    public void ApplyStartTransform()
    {
        startTransformData.ApplyDataToRectTranform(rectTransform);
    }

    public void ApplyGoalTransform()
    {
        goalTransformData.ApplyDataToRectTranform(rectTransform);
    }

    public bool isTraveling()
    {
        return currentTravelTime <= travelTimeLength;
    }

    protected void UpdateTravel()
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
