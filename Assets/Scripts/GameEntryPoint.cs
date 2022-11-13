using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    [System.Serializable]
    public struct UIInitData
    {
        public UIToolboxBucket ToolboxBucket;
        public UIStoryBucket StoryBucket;
        public Canvas RootCanvas;
        public UIWordWidget WordWidgetPrefab;
    }

    public ObjectPoolConfig PoolConfig;

    [Space]

    public UIInitData UIInit;

    [Space]

    public Scenario TestScenarioPrefab;

    private Game game;

    private void Awake()
    {
        game = new Game(PoolConfig, this, UIInit);
        UIInit.RootCanvas.gameObject.SetActive(true);
    }

    private void Start()
    {
        game.ProtoStart(TestScenarioPrefab, transform, UIInit.StoryBucket);
    }

    private void Update()
    {
        game.UpdateMainLoop();
    }

    //called by debug button under main UI
    public void TestPlayback()
    {
        Game.ScenarioController.PlayScenario();
    }
}
