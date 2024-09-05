using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CIISettingsManager : MonoBehaviour
{
    public Vector2Int lensGroupCount = new Vector2Int(1, 1);

    [SerializeField] private Vector2 lensSize = new Vector2(0.48f, 0.48f); // [cm]
    [SerializeField] private float focalLength = 1.5f; // [cm]
    [SerializeField] private float gap = 1.2f; // [cm]
    [SerializeField] private Vector3 eyePosition = new Vector3(0, 0, 60.0f); // [cm]
    [SerializeField] private float near = 1.0f; // [cm]
    [SerializeField] private float far = 50.0f; // [cm]

    [SerializeField] private CIIVisualizer visualizer;

    private void OnValidate()
    {
        CIISettings.lensGroupCount = lensGroupCount;
        CIISettings.lensCount = new Vector2Int(lensGroupCount.x * ElementGroup.MAX_ELEMENT_COUNT_X, lensGroupCount.y * ElementGroup.MAX_ELEMENT_COUNT_Y);
        CIISettings.lensSize = lensSize;
        CIISettings.elementalImageSize = (1.0f + gap / eyePosition.z) * lensSize;
        CIISettings.opticalImageSize = focalLength / (focalLength - gap) * CIISettings.elementalImageSize;
        CIISettings.focalLength = focalLength;
        CIISettings.gap = gap;
        CIISettings.eyePosition = eyePosition;
        CIISettings.near = near;
        CIISettings.far = far;

        if(visualizer != null)
        {
            visualizer.Init();
        }
    }
}
