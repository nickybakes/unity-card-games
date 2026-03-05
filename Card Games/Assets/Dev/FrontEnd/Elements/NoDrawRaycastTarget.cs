using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A basic UI raycast hitbox that has no graphical element to it.
/// </summary>
public class NoDrawRaycastTarget : Graphic
{
    public override void SetVerticesDirty() { return; }

    public override void SetMaterialDirty() { return; }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        return;
    }
}