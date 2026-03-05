using UnityEngine;

/// <summary>
/// Stores the maleable data for a rect tranform without needing a rect transform component.
/// </summary>
public class RectTransformData
{
    /// <summary>
    /// The local anchored position.
    /// </summary>
    public Vector2 anchoredPosition;

    /// <summary>
    /// The local rotation.
    /// </summary>
    public Quaternion rotation;

    /// <summary>
    /// The local scale.
    /// </summary>
    public Vector3 scale;

    /// <summary>
    /// The rect transform of the game object to owns this data.
    /// </summary>
    private RectTransform ownerRect;


    /// <summary>
    /// Constructor initializes the position, rotation, and scale default values.
    /// </summary>
    /// <param name="_ownerRect">The rect transform of the game object to owns this data.</param>
    public RectTransformData(RectTransform _ownerRect)
    {
        anchoredPosition = new Vector2();
        rotation = Quaternion.identity;
        scale = new Vector3();
        ownerRect = _ownerRect;
    }

    /// <summary>
    /// Set the data of this to match a given rect transform.
    /// </summary>
    /// <param name="rect">The rect transform to match.</param>
    public void SetTransformData(RectTransform rect)
    {
        SetTransformData(rect.anchoredPosition, rect.rotation, rect.localScale);
    }

    /// <summary>
    /// Set the position, rotation, and scale.
    /// </summary>
    /// <param name="_anchoredPosition">The position.</param>
    /// <param name="_rotation">The rotation as a Quaternion.</param>
    /// <param name="_scale">The 3D scale.</param>
    public void SetTransformData(Vector2 _anchoredPosition, Quaternion _rotation, Vector3 _scale)
    {
        anchoredPosition = _anchoredPosition;
        rotation = _rotation;
        scale = _scale;
    }

    /// <summary>
    /// Applies the data stored in this to a rect transform.
    /// </summary>
    /// <param name="rect">The rect transform to apply onto.</param>
    public void ApplyDataToRectTranform(RectTransform rect)
    {
        rect.anchoredPosition = anchoredPosition;
        rect.rotation = rotation;
        rect.localScale = scale;
    }

    /// <summary>
    /// Applies a parent's transformations to this rect transform data.
    /// </summary>
    /// <param name="parent">The rect transform to use as a parent.</param>
    /// <param name="applyRotation">Whether to apply the parent's rotation.</param>
    /// <param name="applyScale">Whether to apply the parent's scale.</param>
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