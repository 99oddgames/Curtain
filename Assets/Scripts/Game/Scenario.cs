using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    public ScenarioDefinition ScenarioDefinition;
    public Transform ScenarioContentRoot;

    private Dictionary<string, WordObject> targetableWordsLookup = new Dictionary<string, WordObject>(16);
    private List<WordDefinition> story = new List<WordDefinition>(32);
    private UIStoryBucket storyBucket;

    private VerbState state;
    private VerbDefinition currentAction;

    private int seekIndex = 0;

    public void Initialize(UIStoryBucket storyBucket)
    {
        var targetables = ScenarioContentRoot.GetComponentsInChildren<WordObject>();

        for(int i = 0; i < targetables.Length; i++)
        {
            var nextWordObject = targetables[i];

            if (!targetableWordsLookup.TryAdd(nextWordObject.WordDefinition.Text, nextWordObject))
                Debug.LogWarning($"Scene {name} contains multiple WordObjets with the same WordDefinition ({nextWordObject.WordDefinition}). Only one will be targetable.");

            Game.WidgetFactory.AddWordWidget(nextWordObject.WordDefinition, UIWidgetFactory.EBucket.Toolbox, true);
        }

        for(int i = 0; i < ScenarioDefinition.ToolboxWords.Length; i++)
        {
            Game.WidgetFactory.AddWordWidget(ScenarioDefinition.ToolboxWords[i].Word, UIWidgetFactory.EBucket.Toolbox, true);
        }

        for (int i = 0; i < ScenarioDefinition.StoryWords.Length; i++)
        {
            var nextWord = ScenarioDefinition.StoryWords[i];
            story.Add(nextWord.Word);
            Game.WidgetFactory.AddWordWidget(nextWord.Word, UIWidgetFactory.EBucket.Story, true);

            if (nextWord.Word.WordType != WordDefinition.EWordType.Noun)
                continue;

            if (!targetableWordsLookup.TryGetValue(nextWord.Word.Text, out var match))
                continue;

            nextWord.Word.Owner = match;
        }

        this.storyBucket = storyBucket; 
        enabled = false;
    }

    //quick test code, data will come from elsewhere later
    private void RebuildStory()
    {
        story.Clear();
        
        for(int i = 0; i < storyBucket.Items.Count; i++)
        {
            story.Add(storyBucket.Items[i].WordDefinition);
        }
    }

    public void StartPlayback()
    {
        RebuildStory();
        seekIndex = 0;
        enabled = true;
        currentAction = null;
    }

    private void Update()
    {
        if (currentAction == null)
        {
            if (SeekWord(WordDefinition.EWordType.Noun, story, out var subject, ref seekIndex))
            {
                if (SeekWord(WordDefinition.EWordType.Verb, story, out var verb, ref seekIndex))
                {
                    currentAction = (VerbDefinition)verb;

                    state = new VerbState()
                    {
                        Actor = subject.Owner,
                    };

                    if (SeekWord(WordDefinition.EWordType.Noun, story, out var target, ref seekIndex))
                    {
                        state.Target = target.Owner;
                    }

                    currentAction.StartAction(ref state);
                }
            }

            if (currentAction == null || state.IsDone)
            {
                StopPlayback();
                return;
            }
        }

        currentAction.UpdateAction(ref state);

        if (state.IsDone)
        {
            currentAction.StopAction(ref state);
            currentAction = null;
        }
    }

    private bool SeekWord(WordDefinition.EWordType type, List<WordDefinition> words, out WordDefinition word, ref int index)
    {
        for (int i = index; i < words.Count; i++)
        {
            if (words[i].WordType != type)
                continue;

            word = words[i];
            index = i;
            return true;
        }

        word = null;
        return false;
    }

    public void StopPlayback()
    {
        enabled = false;
        Debug.Log("Playback DONE");
    }
}
