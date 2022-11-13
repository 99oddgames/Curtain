using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioDefinition : ScriptableObject
{
    [System.Serializable]
    public struct StoryWordEntry
    {
        public bool IsLocked;
        public WordDefinition Word;
    }

    [System.Serializable]
    public struct ToolboxWordEntry
    {
        public WordDefinition Word;
    }

    public StoryWordEntry[] StoryWords;
    public ToolboxWordEntry[] ToolboxWords;
}
