using UnityEngine;

public class RectTransformData
{
    public Vector2 anchoredPosition;
    public Quaternion rotation;
    public Vector3 scale;
    private RectTransform ownerRect;

    public RectTransformData(RectTransform _ownerRect)
    {
        anchoredPosition = new Vector2();
        rotation = Quaternion.identity;
        scale = new Vector3();
        ownerRect = _ownerRect;
    }

    public void SetTransformData(RectTransform rect)
    {
        SetTransformData(rect.anchoredPosition, rect.rotation, rect.localScale);
    }

    public void SetTransformData(Vector2 _anchoredPosition, Quaternion _rotation, Vector3 _scale)
    {
        anchoredPosition = _anchoredPosition;
        rotation = _rotation;
        scale = _scale;
    }

    public void ApplyDataToRectTranform(RectTransform rect)
    {
        rect.anchoredPosition = anchoredPosition;
        rect.rotation = rotation;
        rect.localScale = scale;
    }

    public void ApplyRectTransformParentToData(RectTransform parent, bool applyRotation = true, bool applyScale = true)
    {
        RectTransform currentParent = parent;
        while (currentParent != null && currentParent != ownerRect.parent)
        {
            if (applyScale)
            {
                scale.x *= currentParent.localScale.x;
                scale.y *= currentParent.localScale.y;
                scale.z *= currentParent.localScale.z;
            }
            anchoredPosition *= currentParent.localScale;

            if (applyRotation)
            {
                rotation *= currentParent.localRotation;
            }
            anchoredPosition = currentParent.localRotation * anchoredPosition;

            anchoredPosition += currentParent.anchoredPosition;

            if (currentParent.parent is RectTransform)
                currentParent = (RectTransform)currentParent.parent;
            else
                currentParent = null;
        }
    }
}