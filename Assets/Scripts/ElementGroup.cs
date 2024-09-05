using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementGroup
{
    public const int MAX_ELEMENT_COUNT_X = 4;
    public const int MAX_ELEMENT_COUNT_Y = 4;

    public Vector2Int groupId;

    public List<List<Element>> elements;

    public Frustum groupFrustum;
    public Vector3 frustumApex;


    public ElementGroup(Vector2Int groupId)
    {
        elements = new List<List<Element>>();
        this.groupId = groupId;
        Vector2Int globalId = new Vector2Int();
        for (int x = 0; x < MAX_ELEMENT_COUNT_X; x++)
        {
            globalId.x = groupId.x * MAX_ELEMENT_COUNT_X + x;
            elements.Add(new List<Element>());
            for (int y = 0; y < MAX_ELEMENT_COUNT_Y; y++)
            {
                globalId.y = groupId.y * MAX_ELEMENT_COUNT_Y + y;
                elements[x].Add(new Element(globalId));
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
        frustumApex = new Vector3(apexX, apexY, apexZ);

        // right-top elements
        Element rtElement = elements[MAX_ELEMENT_COUNT_X - 1][MAX_ELEMENT_COUNT_Y - 1];
        ElementalLens rtLens = rtElement.elementalLens;
        Frustum rtFrustum = rtElement.ciiFrustum.frustum;

        // left-bottom elements
        Element lbElement = elements[0][0];
        ElementalLens lbLens = lbElement.elementalLens;
        Frustum lbFrustum = lbElement.ciiFrustum.frustum;

        // group frustum
        groupFrustum = new Frustum();
        groupFrustum.top = rtFrustum.top + rtLens.Position.y - frustumApex.y;
        groupFrustum.bottom = lbFrustum.bottom + lbLens.Position.y - frustumApex.y;
        groupFrustum.right = rtFrustum.right + rtLens.Position.x - frustumApex.x;
        groupFrustum.left = lbFrustum.left + lbLens.Position.x - frustumApex.x;
        groupFrustum.near = CIISettings.near + apexZ;
        groupFrustum.far = CIISettings.far + apexZ;

    }
}
