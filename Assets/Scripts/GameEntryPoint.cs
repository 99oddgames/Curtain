using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    public ObjectPoolConfig PoolConfig;

    private Game game;

    private void Awake()
    {
        game = new Game(PoolConfig, this);
    }
}
