using UnityEngine;

public interface IBucketItem
{
    public Vector2 Size
    {
        get;
    }

    public RectTransform Rect
    {
        get;
    }
}