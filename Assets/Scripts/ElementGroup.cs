using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementGroup
{
    public const int MAX_ELEMENT_COUNT_X = 4;
    public const int MAX_ELEMENT_COUNT_Y = 4;

    public Vector2Int GroupId { get; private set; }

    public List<List<Element>> Elements { get; private set; }

    public Frustum GroupFrustum { get; private set; }
    public Vector3 FrustumApex { get; private set; }


    public ElementGroup(Vector2Int groupId)
    {
        Elements = new List<List<Element>>();
        GroupId = groupId;
        Vector2Int globalId = new Vector2Int();
        for (int x = 0; x < MAX_ELEMENT_COUNT_X; x++)
        {
            globalId.x = GroupId.x * MAX_ELEMENT_COUNT_X + x;
            Elements.Add(new List<Element>());
            for (int y = 0; y < MAX_ELEMENT_COUNT_Y; y++)
            {
                globalId.y = GroupId.y * MAX_ELEMENT_COUNT_Y + y;
                Elements[x].Add(new Element(globalId));
            }
        }

        // group frustum apex
        Vector3 eye = CIISettings.eyePosition;
        float gap = CIISettings.gap;
        Vector2 lensSize = CIISettings.lensSize;
        Vector2 lensCount = CIISettings.lensCount;
        int maxElementcount = Mathf.Max(MAX_ELEMENT_COUNT_X, MAX_ELEMENT_COUNT_Y);
        float alpha = (maxElementcount - 1) * gap / (maxElementcount * gap + eye.z);
        float apexX = (1.0f - alpha) * (maxElementcount * groupId.x - (lensCount.x - maxElementcount) / 2.0f) * lensSize.x + alpha * eye.x;
        float apexY = (1.0f - alpha) * (maxElementcount * groupId.y - (lensCount.y - maxElementcount) / 2.0f) * lensSize.y + alpha * eye.y;
        float apexZ = alpha * eye.z;
        FrustumApex = new Vector3(apexX, apexY, apexZ);

        // right-top elements
        Element rtElement = Elements[MAX_ELEMENT_COUNT_X - 1][MAX_ELEMENT_COUNT_Y - 1];
        ElementalLens rtLens = rtElement.elementalLens;
        Frustum rtFrustum = rtElement.ciiFrustum.frustum;

        // left-bottom elements
        Element lbElement = Elements[0][0];
        ElementalLens lbLens = lbElement.elementalLens;
        Frustum lbFrustum = lbElement.ciiFrustum.frustum;

        // group frustum
        GroupFrustum = new Frustum();
        GroupFrustum.top = rtFrustum.top + rtLens.Position.y - FrustumApex.y;
        GroupFrustum.bottom = lbFrustum.bottom + lbLens.Position.y - FrustumApex.y;
        GroupFrustum.right = rtFrustum.right + rtLens.Position.x - FrustumApex.x;
        GroupFrustum.left = lbFrustum.left + lbLens.Position.x - FrustumApex.x;
        GroupFrustum.near = CIISettings.near + apexZ;
        GroupFrustum.far = CIISettings.far + apexZ;

    }
}
