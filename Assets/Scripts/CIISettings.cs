using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CIISettings
{
    public static Vector2Int lensGroupCount = new Vector2Int(1, 1);
    public static Vector2Int lensCount = new Vector2Int(4, 4);
    public static Vector2 lensSize = new Vector2(0.48f, 0.48f); // [cm]
    public static Vector2 elementalImageSize = new Vector2(0.48f, 0.48f); // [cm]
    public static Vector2 opticalImageSize = new Vector2(0.48f, 0.48f); // [cm]
    public static float focalLength = 1.5f; // [cm]
    public static float gap = 1.2f; // [cm]
    public static Vector3 eyePosition = new Vector3(0, 0, 60.0f); // [cm]
    public static float near = 1.0f; // [cm]
    public static float far = 50.0f; // [cm]
}
