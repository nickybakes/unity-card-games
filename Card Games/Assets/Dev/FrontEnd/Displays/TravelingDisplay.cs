using UnityEngine;

public class TravelingDisplay : Display
{

    private RectTransformData startTransformData;

    private RectTransformData goalTransformData;

    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private float travelTimeLength = .2f;

    private float currentTravelTime;

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

    public void StartMoving()
    {
        currentTravelTime = 0;
    }

    public void StopMoving()
    {
        currentTravelTime = travelTimeLength + 1;
    }

    public void SetStartTransform(Vector2 anchoredPosition, Quaternion rotation, Vector3 scale)
    {
        startTransformData.anchoredPosition = anchoredPosition;
        startTransformData.rotation = rotation;
        startTransformData.scale = scale;
    }

    public void SetGoalTransform(Vector2 anchoredPosition, Quaternion rotation, Vector3 scale)
    {
        goalTransformData.anchoredPosition = anchoredPosition;
        goalTransformData.rotation = rotation;
        goalTransformData.scale = scale;
    }

    public void ApplyTransformParentToStartTransform(RectTransform parent, bool applyScale = true, bool applyRotation = true)
    {
        startTransformData.ApplyRectTransformParentToData(parent, applyScale, applyRotation);
    }

    public void ApplyTransformParentToGoalTransform(RectTransform parent, bool applyScale = true, bool applyRotation = true)
    {
        goalTransformData.ApplyRectTransformParentToData(parent, applyScale, applyRotation);
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
        }
        else
        {
            ApplyGoalTransform();
        }
    }
}
