using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PoolService
{
    private Dictionary<PoolableObject, ObjectPool<PoolableObject>> pools = new Dictionary<PoolableObject, ObjectPool<PoolableObject>>(25);
    private Transform root;

    private const int prewarmStep = 10;
    private const int prewarmCapacity = 5;

    public PoolService(ObjectPoolConfig config, MonoBehaviour coroutineHost)
    {
        root = new GameObject("Pool").transform;
        GameObject.DontDestroyOnLoad(root);
        coroutineHost.StartCoroutine(PrewarmRoutine(config.PrewarmObjects));
    }

    private IEnumerator PrewarmRoutine(PoolableObject[] prewarmObjects)
    {
        for(int i = 0; i < prewarmObjects.Length; i++)
        {
            var nextObject = prewarmObjects[i];
            Prewarm(nextObject, prewarmCapacity);

            if (i == 0)
                continue;

            if (i % prewarmStep == 0)
                yield return null;
        }
    }

    public T SafeSpawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool activate = true) where T : PoolableObject
    {
        if (prefab == null)
            return null;

        return Spawn(prefab, position, rotation, parent, activate);
    }

    public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool activate = true) where T : PoolableObject
    {
        if (prefab == null)
        {
            throw new MissingReferenceException("[MonoPool] Failed to spawn an object. Prefab is null.");
        }

        var pool = GetPool(prefab);

        T instance = (T)pool.Spawn(position, rotation, activate);
        SetParent(instance.transform, parent);

        return instance;
    }

    public void Prewarm<T>(T prefab, int count) where T : PoolableObject
    {
        if (prefab == null)
        {
            throw new MissingReferenceException("[MonoPool] Failed to spawn an object. Prefab is null.");
        }

        var pool = GetPool(prefab);
        pool.Prewarm(count, root);
    }

    public void Despawn(PoolableObject instance)
    {
        if (instance.IsDesignTimeObject)
        {
            instance.gameObject.SetActive(false);
            return;
        }

        var pool = GetPool(instance.Prefab);
        pool.Despawn(instance);
        SetParent(instance.transform, root);
    }

    public void DespawnAll()
    {
        foreach (var prefabPoolPair in pools)
        {
            prefabPoolPair.Value.DespawnAll();
        }
    }

    private ObjectPool<PoolableObject> GetPool(PoolableObject prefab)
    {
        if (pools.ContainsKey(prefab))
        {
            return pools[prefab];
        }
        else
        {
            ObjectPool<PoolableObject> newPool = new ObjectPool<PoolableObject>(prefab);
            pools.Add(prefab, newPool);

            return newPool;
        }
    }

    private void SetParent(Transform poolable, Transform parent)
    {
        poolable.SetParent(parent == null ? root : parent);
    }
}
