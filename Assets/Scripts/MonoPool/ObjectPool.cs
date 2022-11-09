using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : PoolableObject
{
    private T m_prefab = null;

    private List<T> m_pool = new List<T>(25);
    private List<T> m_deployed = new List<T>(25);

    public T Prefab
    {
        get
        {
            return m_prefab;
        }
    }

    public ObjectPool(T prefab)
    {
        this.m_prefab = prefab;
    }

    public void Prewarm(int count, Transform parent)
    {
        try
        {
            m_prefab.gameObject.SetActive(false);

            for (int i = 0; i < count; i++)
            {
                T instance = GameObject.Instantiate(m_prefab, parent);
                instance.Prefab = m_prefab;
                instance.IsDesignTimeObject = false;

                m_pool.Add(instance);
            }
        }
        finally
        {
            m_prefab.gameObject.SetActive(true);
        }
    }

    public T Spawn(Vector3 position, Quaternion rotation, bool activate)
    {
        if (m_pool.Count > 0)
        {
            T instance = m_pool[m_pool.Count - 1];

            instance.transform.position = position;
            instance.transform.rotation = rotation;

            m_pool.RemoveAt(m_pool.Count - 1);
            m_deployed.Add(instance);

            instance.OnSpawned();
            instance.Reinitialize();

            if (activate)
            {
                instance.gameObject.SetActive(true);
            }

            return instance;
        }
        else
        {
            try
            {
                m_prefab.gameObject.SetActive(false);

                T instance = GameObject.Instantiate(m_prefab, position, rotation);
                instance.Prefab = m_prefab;
                instance.IsDesignTimeObject = false;
                instance.OnSpawned();
                instance.Reinitialize();

                if (activate)
                {
                    instance.gameObject.SetActive(true);
                }

                m_deployed.Add(instance);
                return instance;
            }
            finally
            {
                m_prefab.gameObject.SetActive(true);
            }
        }
    }

    public void Despawn(T instance)
    {
        instance.gameObject.SetActive(false);
        instance.OnDespawned();

        if (!m_deployed.Contains(instance))
        {
            Debug.Log("Design time object despawned.");
        }
        else
        {
            m_deployed.Remove(instance);
        }

        m_pool.Add(instance);
    }

    public void DespawnAll()
    {
        for (int i = m_deployed.Count - 1; i >= 0; i--)
        {
            m_deployed[i].Despawn();
        }
    }
}
