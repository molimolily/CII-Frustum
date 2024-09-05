using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CIIVisualizer : MonoBehaviour
{
    bool isInitialized = false;
    List<List<ElementGroup>> groupList;

    public void Init()
    {
        groupList = new List<List<ElementGroup>>();
        for (int i = 0; i < CIISettings.lensGroupCount.x; i++)
        {
            groupList.Add(new List<ElementGroup>());
            for (int j = 0; j < CIISettings.lensGroupCount.y; j++)
            {
                groupList[i].Add(new ElementGroup(new Vector2Int(i, j)));
            }
        }

        isInitialized = true;
    }

    private void OnDrawGizmos()
    {
        if (!isInitialized)
        {
            return;
        }

        Draw();
    }

    void Draw()
    {
        // Draw Eye
        Gizmos.color = Color.red;
        Vector3 eyePositionWS = transform.TransformPoint(CIISettings.eyePosition);
        Gizmos.DrawWireSphere(eyePositionWS, 0.5f);


        // Draw Elements
        foreach (var rowGroup in groupList)
        {
            foreach (var group in rowGroup)
            {
                foreach (var rowElements in group.elements)
                {
                    foreach (var element in rowElements)
                    {
                        // Draw lens
                        Gizmos.color = Color.cyan;
                        Vector2 lensSize = CIISettings.lensSize;
                        ElementalLens lens = element.elementalLens;
                        GizmoUtil.DrawEllipse(lens.Position, transform, lensSize);

                        // Draw elemental image
                        Gizmos.color = Color.green;
                        Vector2 elementalImageSize = CIISettings.elementalImageSize;
                        ElementalImage elementalImage = element.elementalImage;
                        GizmoUtil.DrawQuad(elementalImage.Position, transform, elementalImageSize);

                        // Draw optical image
                        Gizmos.color = Color.blue;
                        Vector2 opticalImageSize = CIISettings.opticalImageSize;
                        OpticalImage opticalImage = element.opticalImage;
                        GizmoUtil.DrawQuad(opticalImage.Position, transform, opticalImageSize);

                        // Draw frustum
                        Gizmos.color = new Color(0.8f, 0.2f, 0.0f, 0.2f);
                        Frustum frustum = element.ciiFrustum.frustum;
                        GizmoUtil.DrawFrustum(frustum, transform, lens.Position);

                        // Draw Optical Axis
                        Gizmos.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.1f);
                        Vector3 eyePosition = CIISettings.eyePosition;
                        Vector3 dir = lens.Position - eyePosition;
                        dir.Normalize();
                        float length = - (eyePosition.z + CIISettings.far) / dir.z;
                        Vector3 from = eyePositionWS;
                        Vector3 to = transform.TransformPoint(eyePosition + length * dir);
                        Gizmos.DrawLine(from, to);

                    }
                }

                // Draw Group Frustum
                Gizmos.color = new Color(Color.magenta.r, Color.magenta.g, Color.magenta.b, 0.2f);
                Frustum groupFrustum = group.groupFrustum;
                Vector3 apex = group.frustumApex;
                GizmoUtil.DrawFrustum(groupFrustum, transform, apex);
            }
        }

        Gizmos.color = Color.white;
    }
}
