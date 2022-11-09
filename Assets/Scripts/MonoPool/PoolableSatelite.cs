using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableSatelite : MonoBehaviour
{
    private PoolableObject poolable;

    private void Awake()
    {
        poolable = GetComponentInParent<PoolableObject>();
        poolable.PreDespawn += Return;
    }

    private void OnEnable()
    {
        transform.SetParent(poolable.transform.parent);
    }

    private void Return()
    {
        transform.SetParent(poolable.transform);
    }
}
