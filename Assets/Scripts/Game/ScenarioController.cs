using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController
{
    private Scenario currentScenario;

    public Scenario SpawnScenario(Scenario prefab, Transform root, UIStoryBucket storyBucket)
    {
        if(currentScenario != null)
        {
            GameObject.Destroy(currentScenario);
        }

        currentScenario = GameObject.Instantiate(prefab, root);
        currentScenario.transform.localPosition = Vector3.zero;
        currentScenario.Initialize(storyBucket);

        var rewindables = currentScenario.GetComponentsInChildren<IRewindableObject>();
        
        for(int i = 0; i < rewindables.Length; i++)
        {
            rewindables[i].RecordState();
        }

        return currentScenario;
    }

    public void PlayScenario()
    {
        RewindScenario();
        currentScenario.StartPlayback();
    }

    public void RewindScenario()
    {
        var rewindables = currentScenario.GetComponentsInChildren<IRewindableObject>();

        for (int i = 0; i < rewindables.Length; i++)
        {
            rewindables[i].Rewind();
        }
    }
}
