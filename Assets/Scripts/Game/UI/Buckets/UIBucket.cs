using System.Collections.Generic;
using UnityEngine;

public class UIBucket<T> : MonoBehaviour, IDragAndDropBucket<T> where T : IBucketItem
{
    public float LeftPadding = 50f;
    public float TopPadding = 50f;
    public float Spacing = 0f;
    public RectTransform LocalPivot;

    public bool Horizontal = true;

    private List<T> items = new List<T>(16);

    private int insertIndex = -1;
    private int prevIndex = -1;

    public RectTransform Rect => rect == null ? rect = GetComponent<RectTransform>() : rect;
    private RectTransform rect;

    public bool Contains(T item) => items.Contains(item);

    void Update()
    {
        var pos = LocalPivot.localPosition; //bypass ugui size math being stupid for stretched rects

        for (int i = 0; i < items.Count; i++) //this will need to be much more robust later(grid stacking etc.)
        {
            var nextItem = items[i];
            var rect = nextItem.Rect;

            var offsetX = Horizontal ? nextItem.Size.x + Spacing : 0f;
            var offsetY = Horizontal ? 0f : -nextItem.Size.y - Spacing;
            var insertSpace = i == insertIndex ? new Vector3(offsetX, offsetY) : Vector3.zero;
            pos += insertSpace;

            var desiredPos = pos + new Vector3(LeftPadding, -TopPadding) + new Vector3(offsetX * 0.5f, offsetY * 0.5f);
            rect.localPosition = Vector3.Lerp(rect.localPosition, desiredPos, Time.deltaTime * 18f);

            pos += new Vector3(offsetX, offsetY);
        }
    }

    public bool OnDrop(Vector3 position, T item)
    {
        AddItem(item, insertIndex >= 0 ? insertIndex : prevIndex);
        insertIndex = -1;
        return true;
    }

    public void OnExit(T item)
    {
        insertIndex = -1;
    }

    public void OnHover(Vector3 position, T item)
    {
        var closestItem = GetClosestContainedItem(position, out insertIndex);

        if (closestItem == null)
            return;

        if(Horizontal)
        {
            insertIndex += position.x > closestItem.Rect.position.x ? 1 : 0;
        }
        else
        {
            insertIndex += position.y > closestItem.Rect.position.y ? 0 : 1;
        }
    }

    public void OnPickUp(T item)
    {
        if (!items.Contains(item))
            return;

        prevIndex = items.IndexOf(item);
        items.Remove(item);
    }

    public void AddItem(T item, int index = -1)
    {
        if (index >= 0)
        {
            items.Insert(index, item);
        }
        else
        {
            items.Add(item);
        }

        item.Rect.SetParent(transform);
    }

    public void RemoveItem(T item)
    {
        items.Remove(item);
    }

    private T GetClosestContainedItem(Vector3 point, out int index)
    {
        var min = float.MaxValue;
        T result = default;
        index = -1;

        for (int i = 0; i < items.Count; i++)
        {
            var nextItem = items[i];
            var sqm = (nextItem.Rect.position - point).sqrMagnitude;

            if (sqm > min)
                continue;

            min = sqm;
            result = nextItem;
            index = i;
        }

        return result;
    }
}
