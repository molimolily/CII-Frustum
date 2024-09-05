using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalImage
{
    public Vector3 Position { get; private set; }

    public ElementalImage(Vector3 lensPosition)
    {
        SetPosition(lensPosition);
    }

    void SetPosition(Vector3 lensPosition)
    {
        float g = CIISettings.gap;
        Vector3 eye = CIISettings.eyePosition;
        float t = - g / eye.z;
        Position = (1.0f - t) * lensPosition + t * eye;
    }
}
