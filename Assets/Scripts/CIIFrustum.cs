using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frustum
{
    public float top;
    public float bottom;
    public float left;
    public float right;
    public float near;
    public float far;
}

public class CIIFrustum
{
    public Frustum frustum;

    public CIIFrustum(Vector2Int id)
    {
        Vector2Int lensCount = CIISettings.lensCount;
        Vector2 lensSize = CIISettings.lensSize;
        Vector3 eye = CIISettings.eyePosition;
        float gap = CIISettings.gap;
        float near = CIISettings.near;
        float far = CIISettings.far;

        frustum = new Frustum();
        frustum.top = ((id.y - lensCount.y / 2.0f + 1) / eye.z + 0.5f / gap) * near * lensSize.y - eye.y * near / eye.z;
        frustum.bottom = ((id.y - lensCount.y / 2.0f) / eye.z - 0.5f / gap) * near * lensSize.y - eye.y * near / eye.z;
        frustum.right = ((id.x - lensCount.x / 2.0f + 1) / eye.z + 0.5f / gap) * near * lensSize.x - eye.x * near / eye.z;
        frustum.left = ((id.x - lensCount.x / 2.0f) / eye.z - 0.5f / gap) * near * lensSize.x - eye.x * near / eye.z;
        frustum.near = near;
        frustum.far = far;
    }
}
