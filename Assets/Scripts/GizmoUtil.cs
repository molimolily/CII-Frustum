using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmoUtil
{
    public static void DrawEllipse(Vector3 localPosition, Transform transform, float width, float height, int segments = 36)
    {
        Vector3[] points = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            points[i] = localPosition + new Vector3(Mathf.Cos(angle) * width / 2, Mathf.Sin(angle) * height / 2, 0);
        }
        transform.TransformPoints(points); // local to world
        Gizmos.DrawLineStrip(points, true);
    }

    public static void DrawEllipse(Vector3 localPosition, Transform transform, Vector2 size, int segments = 36)
    {
        DrawEllipse(localPosition, transform, size.x, size.y, segments);
    }

    public static void DrawQuad(Vector3 localPosition, Transform transform, float width, float height)
    {
        Vector3[] points = new Vector3[4];
        points[0] = localPosition + new Vector3(-width / 2, -height / 2, 0);
        points[1] = localPosition + new Vector3(width / 2, -height / 2, 0);
        points[2] = localPosition + new Vector3(width / 2, height / 2, 0);
        points[3] = localPosition + new Vector3(-width / 2, height / 2, 0);
        transform.TransformPoints(points); // local to world
        Gizmos.DrawLineStrip(points, true);
    }

    public static void DrawQuad(Vector3 localPosition, Transform transform, Vector2 size)
    {
        DrawQuad(localPosition, transform, size.x, size.y);
    }

    public static void DrawFrustum(Frustum frustum, Transform transform, Vector3 apex = default(Vector3))
    {
        Vector3[] nearPlane = new Vector3[4];
        nearPlane[0] = apex + new Vector3(frustum.left, frustum.bottom, -frustum.near);
        nearPlane[1] = apex + new Vector3(frustum.right, frustum.bottom, -frustum.near);
        nearPlane[2] = apex + new Vector3(frustum.right, frustum.top, -frustum.near);
        nearPlane[3] = apex + new Vector3(frustum.left, frustum.top, -frustum.near);

        Vector3[] farPlane = new Vector3[4];
        farPlane[0] = apex + new Vector3(frustum.left * frustum.far / frustum.near, frustum.bottom * frustum.far / frustum.near, -frustum.far);
        farPlane[1] = apex + new Vector3(frustum.right * frustum.far / frustum.near, frustum.bottom * frustum.far / frustum.near, -frustum.far);
        farPlane[2] = apex + new Vector3(frustum.right * frustum.far / frustum.near, frustum.top * frustum.far / frustum.near, -frustum.far);
        farPlane[3] = apex + new Vector3(frustum.left * frustum.far / frustum.near, frustum.top * frustum.far / frustum.near, -frustum.far);

        transform.TransformPoints(nearPlane);
        transform.TransformPoints(farPlane);

        Gizmos.DrawLineStrip(nearPlane, true);
        Gizmos.DrawLineStrip(farPlane, true);

        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(nearPlane[i], farPlane[i]);
        }
    }
}
