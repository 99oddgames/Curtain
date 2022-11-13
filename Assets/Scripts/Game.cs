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

    public static UIWidgetFactory WidgetFactory
    {
        get;
        private set;
    }

    public static ScenarioController ScenarioController
    {
        get;
        private set;
    }

    private MonoBehaviour coroHost;

    public Game(ObjectPoolConfig poolConfig, MonoBehaviour coroutineHost, GameEntryPoint.UIInitData uiInitData)
    {
        ObjectPool = new PoolService(poolConfig, coroutineHost);
        WidgetFactory = new UIWidgetFactory(uiInitData);
        ScenarioController = new ScenarioController();
        coroHost = coroutineHost;
    }

    //test code
    public void ProtoStart(Scenario scenario, Transform root, UIStoryBucket storyBucket)
    {
        ScenarioController.SpawnScenario(scenario, root, storyBucket);
    }

    public void UpdateMainLoop(){ }
}
