using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public static PoolService ObjectPool
    {
        get;
        private set;
    }

    public Game(ObjectPoolConfig poolConfig, MonoBehaviour coroutineHost)
    {
        ObjectPool = new PoolService(poolConfig, coroutineHost);
    }
}
