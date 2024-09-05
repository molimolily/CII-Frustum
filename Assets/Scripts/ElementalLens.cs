using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalLens
{
    public Vector3 Position { get; private set; }

    public ElementalLens(Vector2Int id)
    {
        SetPosition(id);
    }

    void SetPosition(Vector2Int id)
    {
        Vector2Int count = CIISettings.lensCount;
        Vector2 size = CIISettings.lensSize;

        Position = new Vector3(
            (id.x - (count.x - 1) / 2.0f) * size.x,
            (id.y - (count.y - 1) / 2.0f) * size.y
            );
    }
}
