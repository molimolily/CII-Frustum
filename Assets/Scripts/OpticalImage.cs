using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpticalImage
{
    public Vector3 Position { get; private set; }

    public OpticalImage(Vector3 lensPosition)
    {
        SetPosition(lensPosition);
    }

    void SetPosition(Vector3 lensPosition)
    {
        float g = CIISettings.gap;
        float f = CIISettings.focalLength;
        Vector3 eye = CIISettings.eyePosition;
        float t = -f * g / (eye.z * (f - g));
        Position = (1.0f - t) * lensPosition + t * eye;
    }
}
