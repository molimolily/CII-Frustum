using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element
{
    public Vector2Int id;
    public ElementalLens elementalLens;
    public ElementalImage elementalImage;
    public OpticalImage opticalImage;
    public CIIFrustum ciiFrustum;

    public Element(Vector2Int id)
    {
        this.id = id;
        elementalLens = new ElementalLens(id);
        elementalImage = new ElementalImage(elementalLens.Position);
        opticalImage = new OpticalImage(elementalLens.Position);
        ciiFrustum = new CIIFrustum(id);
    }
}
