using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CIIFrustum : MonoBehaviour
{
    class Frustum
    {
        public float top;
        public float bottom;
        public float left;
        public float right;
        public float near;
        public float far;
    }

    [SerializeField] Vector2Int lensCount = new Vector2Int(4, 4);
    [SerializeField] Vector2 lensSize = new Vector2(0.0048f, 0.0048f);
    [SerializeField] float focalLength = 0.015f;
    [SerializeField] float gap = 0.012f;

    [SerializeField] Vector3 eyePosition = new Vector3(0, 0, 0.6f);

    [SerializeField] float near = 0.01f;
    [SerializeField] float far = 0.5f;

    List<List<Vector3>> lensPositions = new List<List<Vector3>>();
    List<List<Vector3>> elementalImagePositions = new List<List<Vector3>>();
    List<List<Vector3>> opticalImagePositions = new List<List<Vector3>>();
    List<List<Frustum>> frustums = new List<List<Frustum>>();

    private void OnValidate()
    {
        Init();
    }

    private void OnDrawGizmos()
    {
        DrawLensArray();
        DrawElementalImages();
        DrawOpticalImages();
        DrawEye();
        DrawCIIFrustums();
    }

    void Init()
    {
        SetLensPositions();
        SetElementalImagePositions();
        SetOpticalImagePositions();
        SetFrustums();
    }

    void SetLensPositions()
    {
        lensPositions.Clear();

        for(int i = 0; i < lensCount.x; i++)
        {
            lensPositions.Add(new List<Vector3>());
            for(int j = 0; j < lensCount.y; j++)
            {
                Vector3 lensPosition = new Vector3((i - (lensCount.x - 1) / 2) * lensSize.x, (j - (lensCount.y - 1) / 2) * lensSize.y, 0.0f);
                lensPositions[i].Add(lensPosition);
            }
        }
    }

    void SetElementalImagePositions()
    {
        elementalImagePositions.Clear();

        for(int i = 0; i < lensCount.x; i++)
        {
            elementalImagePositions.Add(new List<Vector3>());
            for(int j = 0; j < lensCount.y; j++)
            {
                Vector3 lensPosition = lensPositions[i][j];
                float t = -gap / eyePosition.z;
                Vector3 pos = (1.0f - t) * lensPosition + t * eyePosition;
                elementalImagePositions[i].Add(pos);
            }
        }
    }

    void SetOpticalImagePositions()
    {
        opticalImagePositions.Clear();

        for(int i = 0; i < lensCount.x; i++)
        {
            opticalImagePositions.Add(new List<Vector3>());
            for(int j = 0; j < lensCount.y; j++)
            {
                Vector3 lensPosition = lensPositions[i][j];
                float t = -focalLength * gap / (focalLength - gap) / eyePosition.z;
                Vector3 pos = (1.0f - t) * lensPosition + t * eyePosition;
                opticalImagePositions[i].Add(pos);
            }
        }
    }

    void SetFrustums()
    {
        frustums.Clear();
        for (int i = 0; i < lensCount.x; i++)
        {
            frustums.Add(new List<Frustum>());
            for (int j = 0; j < lensCount.y; j++)
            {
                Frustum frustum = new Frustum();
                frustum.near = near;
                frustum.far = far;
                frustum.right = ((i - lensCount.x / 2 + 1) / eyePosition.z + 0.5f / gap) * near * lensSize.x - eyePosition.x * near / eyePosition.z;
                frustum.left = ((i - lensCount.x / 2) / eyePosition.z - 0.5f / gap) * near * lensSize.x - eyePosition.x * near / eyePosition.z;
                frustum.top = ((j - lensCount.y / 2 + 1) / eyePosition.z + 0.5f / gap) * near * lensSize.y - eyePosition.y * near / eyePosition.z;
                frustum.bottom = ((j - lensCount.y / 2) / eyePosition.z - 0.5f / gap) * near * lensSize.y - eyePosition.y * near / eyePosition.z;
                frustums[i].Add(frustum);
            }
        }
    }

    void DrawEllipse(Vector3 localPosition, float width, float height, int segments = 36)
    {
        Vector3[] points = new Vector3[segments];
        for(int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            points[i] = localPosition + new Vector3(Mathf.Cos(angle) * width / 2, Mathf.Sin(angle) * height / 2, 0);
        }
        transform.TransformPoints(points); // local to world
        Gizmos.DrawLineStrip(points, true);
    }

    void DrawQuad(Vector3 localPosition, float width, float height)
    {
        Vector3[] points = new Vector3[4];
        points[0] = localPosition + new Vector3(-width / 2, -height / 2, 0);
        points[1] = localPosition + new Vector3(width / 2, -height / 2, 0);
        points[2] = localPosition + new Vector3(width / 2, height / 2, 0);
        points[3] = localPosition + new Vector3(-width / 2, height / 2, 0);

        transform.TransformPoints(points); // local to world

        Gizmos.DrawLineStrip(points, true);
    }

    void DrawFrustum(Frustum frustum, Vector3 apex = default(Vector3))
    {
        Vector3[] nearPlane = new Vector3[4];
        nearPlane[0] = apex + new Vector3(frustum.left, frustum.bottom, -frustum.near);
        nearPlane[1] = apex + new Vector3(frustum.right, frustum.bottom, -frustum.near);
        nearPlane[2] = apex + new Vector3(frustum.right, frustum.top, -frustum.near);
        nearPlane[3] = apex + new Vector3(frustum.left, frustum.top, -frustum.near);

        Vector3[] farPlane = new Vector3[4];
        farPlane[0] = apex + new Vector3(frustum.left * far / near, frustum.bottom * far / near, -frustum.far);
        farPlane[1] = apex + new Vector3(frustum.right * far / near, frustum.bottom * far / near, -frustum.far);
        farPlane[2] = apex + new Vector3(frustum.right * far / near, frustum.top * far / near, -frustum.far);
        farPlane[3] = apex + new Vector3(frustum.left * far / near, frustum.top * far / near, -frustum.far);

        transform.TransformPoints(nearPlane);
        transform.TransformPoints(farPlane);

        Gizmos.DrawLineStrip(nearPlane, true);
        Gizmos.DrawLineStrip(farPlane, true);

        for(int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(nearPlane[i], farPlane[i]);
        }
    }

    void DrawLensArray()
    {
        Gizmos.color = Color.cyan;

        foreach (var row in lensPositions)
        {
            foreach (var lensPosition in row)
            {
                DrawEllipse(lensPosition, lensSize.x, lensSize.y);
            }
        }

        Gizmos.color = Color.white;
    }

    void DrawElementalImages()
    {
        Gizmos.color = Color.green;

        float width = lensSize.x * (1.0f + gap / eyePosition.z);
        float height = lensSize.y * (1.0f + gap / eyePosition.z);

        foreach (var row in elementalImagePositions)
        {
            foreach (var pos in row)
            {
                DrawQuad(pos, width, height);
            }
        }

        Gizmos.color = Color.white;
    }

    void DrawOpticalImages()
    {
        Gizmos.color = Color.blue;

        float width = lensSize.x * (1.0f + gap / eyePosition.z) * focalLength / (focalLength - gap);
        float height = lensSize.y * (1.0f + gap / eyePosition.z) * focalLength / (focalLength - gap);

        foreach (var row in opticalImagePositions)
        {
            foreach (var pos in row)
            {
                DrawQuad(pos, width, height);
            }
        }

        Gizmos.color = Color.white;
    }

    void DrawEye()
    {
        Gizmos.color = Color.red;
        // 視点位置の描画
        Vector3 pos = transform.TransformPoint(eyePosition); // local to world
        Gizmos.DrawWireSphere(pos, 0.5f);

        // 視点位置からレンズ中心を通る直線の描画
        Gizmos.color = new Color(0.8f, 0.8f, 0.0f);

        for(int i = 0; i < lensCount.x; i++)
        {
            for(int j = 0; j < lensCount.y; j++)
            {
                Vector3 dir = lensPositions[i][j] - eyePosition;
                dir.Normalize();
                float length = - (eyePosition.z + far) / dir.z;
                Vector3 from = pos;
                Vector3 to = transform.TransformPoint(eyePosition + length * dir);
                Gizmos.DrawLine(from, to);
            }
        }

        Gizmos.color = Color.white;
    }

    void DrawCIIFrustums()
    {
        Gizmos.color = new Color(0.8f, 0.2f, 0.0f);

        for(int i = 0; i < lensCount.x; i++)
        {
            for(int j = 0; j < lensCount.y; j++)
            {
                Frustum frustum = frustums[i][j];
                Vector3 apex = lensPositions[i][j];
                DrawFrustum(frustum, apex);
            }
        }

        Gizmos.color = Color.white;
    }

}
