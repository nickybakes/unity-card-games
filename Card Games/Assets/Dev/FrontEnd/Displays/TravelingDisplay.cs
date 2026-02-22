using UnityEngine;

public class TravelingDisplay : Display
{
    private Vector2 startAnchoredPosition;
    private Quaternion startRotation;
    private Vector3 startScale;

    private Vector2 goalAnchoredPosition;
    private Quaternion goalRotation;
    private Vector3 goalScale;

    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private float travelTimeLength = .2f;

    private float currentTravelTime;

    void Awake()
    {
        SetupRectTransform();
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
        startAnchoredPosition = anchoredPosition;
        startRotation = rotation;
        startScale = scale;
    }

    public void SetGoalTransform(Vector2 anchoredPosition, Quaternion rotation, Vector3 scale)
    {
        goalAnchoredPosition = anchoredPosition;
        goalRotation = rotation;
        goalScale = scale;
    }

    public void ApplyLerpTransform(float t)
    {
        rectTransform.anchoredPosition = Vector2.LerpUnclamped(startAnchoredPosition, goalAnchoredPosition, t);
        rectTransform.rotation = Quaternion.LerpUnclamped(startRotation, goalRotation, t);
        rectTransform.localScale = Vector3.LerpUnclamped(startScale, goalScale, t);
    }

    public void ApplyGoalTransform()
    {
        rectTransform.anchoredPosition = goalAnchoredPosition;
        rectTransform.rotation = goalRotation;
        rectTransform.localScale = goalScale;
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
