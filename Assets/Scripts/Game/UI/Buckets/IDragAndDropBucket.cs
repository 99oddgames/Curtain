using UnityEngine;

public interface IDragAndDropBucket<T>
{
    public RectTransform Rect { get; }

    public void OnPickUp(T item);
    public void OnHover(Vector3 position, T item);
    public bool OnDrop(Vector3 position, T item);
    public void OnExit(T item);

    public void AddItem(T item, int index = -1, bool snapPositions = false);
    public void RemoveItem(T item);
    public bool Contains(T item);
}
