using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableObject : MonoBehaviour
{
    private PoolableObject m_prefab = null;
    private bool m_isSpawned = false;
    private bool m_isDesignTimeObject = true;

    public System.Action PreDespawn;

    private float m_spawnTimestamp;

    public bool IsSpawned
    {
        get
        {
            return m_isSpawned;
        }
    }

    public bool IsDesignTimeObject
    {
        get
        {
            return m_isDesignTimeObject;
        }
        set
        {
            m_isDesignTimeObject = value;
        }
    }

    public float ElapsedSinceSpawn
    {
        get
        {
            if (!IsSpawned)
            {
                Debug.LogError($"Pooled instance ({name}) is being polled for time ElapsedSinceSpawn while not spawned. This is not supported.");
                return -1;
            }

            return Time.time - m_spawnTimestamp;
        }
    }

    public PoolableObject Prefab
    {
        get
        {
            return m_prefab;
        }
        set
        {
            m_prefab = value;
        }
    }

    public void OnSpawned()
    {
        m_spawnTimestamp = Time.time;
        m_isSpawned = true;
        OnSpawn();
    }

    public void OnDespawned()
    {
        m_isSpawned = false;
    }

    private void Awake()
    {
        OnAwake();
    }

    private void Start()
    {
        if (m_isDesignTimeObject)
        {
            Reinitialize();
            OnSpawned();
        }

        OnStart();
    }

    public virtual void OnAwake() { }
    public virtual void OnStart() { }
    public virtual void Reinitialize() { }
    public virtual void OnDespawn() { }
    public virtual void OnSpawn() { }

    public void Despawn(float delay = 0)
    {
        PreDespawn?.Invoke();

        if (!m_isSpawned)
        {
            if (IsDesignTimeObject)
                gameObject.SetActive(false);

            return;
        }

        if (delay == 0)
        {
            StopAllCoroutines();
            Game.ObjectPool.Despawn(this);
            OnDespawn();

            return;
        }

        StartCoroutine(DespawnRoutine(delay));
    }

    private IEnumerator DespawnRoutine(float delay)
    {
        float elapsed = 0.0f;

        while (elapsed < delay)
        {
            yield return null;
            elapsed += Time.deltaTime;
        }

        StopAllCoroutines();
        Game.ObjectPool.Despawn(this);
        OnDespawn();
    }

    public bool IsActive()
    {
        return gameObject.activeInHierarchy;
    }

    public bool IsNull()
    {
        return this == null;
    }
}

public static class MonoPoolableExtensions
{
    public static T SafeSpawn<T>(this T prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool activate = true) where T : PoolableObject
    {
        return Game.ObjectPool.SafeSpawn(prefab, position, rotation, parent, activate);
    }

    public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool activate = true) where T : PoolableObject
    {
        return Game.ObjectPool.Spawn(prefab, position, rotation, parent, activate);
    }
}
