using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWordWidget : PoolableObject, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IBucketItem
{
    public WordDefinition WordDefinition;
    public TextMeshProUGUI Label;
    public Graphic Background;

    public Vector2 Size => Rect.sizeDelta;
    public RectTransform Rect => rect == null ? rect = GetComponent<RectTransform>() : rect;
    private RectTransform rect;
    private RectTransform rootRect;

    private IDragAndDropBucket<UIWordWidget>[] buckets;
    private IDragAndDropBucket<UIWordWidget> hoveredBucket;
    private IDragAndDropBucket<UIWordWidget> sourceBucket;

    public void Initialize(WordDefinition wordDefinition, GameEntryPoint.UIInitData initData)
    {
        WordDefinition = wordDefinition;
        Label.text = wordDefinition.Text;
        Background.color = wordDefinition.BackgroundColor;

        buckets = new IDragAndDropBucket<UIWordWidget>[] { initData.StoryBucket, initData.ToolboxBucket };
        rootRect = (RectTransform)initData.RootCanvas.transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(TryGetOwnerBucket(out sourceBucket))
        {
            sourceBucket.OnPickUp(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rootRect, eventData.position, eventData.pressEventCamera, out var globalMousePos))
        {
            transform.position = globalMousePos;
            transform.rotation = rootRect.rotation;

            if(TryGetBucketAtPoint(eventData.position, out var bucket))
            {
                bucket.OnHover(eventData.position, this);
                hoveredBucket = bucket;
            }
            else if(hoveredBucket != null)
            {
                hoveredBucket.OnExit(this);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (TryGetBucketAtPoint(eventData.position, out var bucket))
        {
            if (bucket.OnDrop(eventData.position, this))
            {
                hoveredBucket = bucket;

                if(sourceBucket != null && sourceBucket != hoveredBucket)
                {
                    sourceBucket?.RemoveItem(this);
                }

                sourceBucket = null;
                return;
            }
        }

        sourceBucket?.OnDrop(eventData.position, this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //soon
    }

    private bool TryGetBucketAtPoint(Vector3 screenPoint, out IDragAndDropBucket<UIWordWidget> bucket)
    {
        for (int i = 0; i < buckets.Length; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(buckets[i].Rect, screenPoint))
            {
                bucket = buckets[i];
                return true;
            }
        }

        bucket = null;
        return false;
    }

    private bool TryGetOwnerBucket(out IDragAndDropBucket<UIWordWidget> bucket)
    {
        for (int i = 0; i < buckets.Length; i++)
        {
            if (!buckets[i].Contains(this))
                continue;

            bucket = buckets[i];
            return true;
        }

        bucket = null;
        return false;
    }
}
