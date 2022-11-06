using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackEngine : MonoBehaviour
{
    //throwaway test code, everything here will be removed

    public VerbAttack A1_ThisAction_A2;
    private VerbDefinition currentAction;

    public IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        var s = FindObjectOfType<PlaybackScene>();

        var actor = s.GetNounObjects()[0];
        var target = s.GetNounObjects()[1];

        currentAction = A1_ThisAction_A2;

        state = new VerbState()
        {
            Actor = actor.gameObject,
            Target = target.gameObject,
        };

        currentAction.StartAction(ref state);
    }

    private VerbState state;

    private void Update()
    {
        if (currentAction == null)
            return;

        currentAction.UpdateAction(ref state);

        if(state.IsDone)
        {
            currentAction.StopAction(ref state);
            currentAction = null;
        }
    }
}
