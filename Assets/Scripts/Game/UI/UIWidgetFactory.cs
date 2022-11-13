using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWidgetFactory
{
    public enum EBucket
    {
        Toolbox,
        Story
    }

    private List<UIWordWidget> spawnedWordWidgets = new List<UIWordWidget>(32);
    private GameEntryPoint.UIInitData uiInitData;

    public UIWidgetFactory(GameEntryPoint.UIInitData uiInitData)
    {
        this.uiInitData = uiInitData;
    }

    private IDragAndDropBucket<UIWordWidget> GetBucket(EBucket bucket) => bucket switch
    {
        EBucket.Toolbox => uiInitData.ToolboxBucket,
        EBucket.Story => uiInitData.StoryBucket,
        _ => throw new System.ArgumentOutOfRangeException(nameof(bucket), $"Unexpected value: {bucket}")
    };


    public void AddWordWidget(WordDefinition definition, EBucket bucketType, bool snapPositions = false)
    {
        var bucket = GetBucket(bucketType);
        var instance = uiInitData.WordWidgetPrefab.Spawn(Vector2.zero, Quaternion.identity, bucket.Rect);
        instance.Initialize(definition, uiInitData);
        bucket.AddItem(instance, -1, snapPositions);
        spawnedWordWidgets.Add(instance);
    }

    public void ClearWordWidgets()
    {
        for (int i = 0; i < spawnedWordWidgets.Count; i++)
        {
            spawnedWordWidgets[i].Despawn();
        }

        spawnedWordWidgets.Clear();
    }
}
