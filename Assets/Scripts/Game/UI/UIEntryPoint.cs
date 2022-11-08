using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEntryPoint : MonoBehaviour
{
    private void Start()
    {
        var wordBuckets = GetComponentsInChildren<IDragAndDropBucket<UIWordWidget>>();
        var rootCanvas = GetComponent<Canvas>();

        EventBetter.Raise(new UIEvents.UIInitialize()
        {
            WordBuckets = wordBuckets,
            RootCanvas = rootCanvas
        });
    }
}
