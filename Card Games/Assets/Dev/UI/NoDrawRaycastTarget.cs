using UnityEngine;
using UnityEngine.UI;

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