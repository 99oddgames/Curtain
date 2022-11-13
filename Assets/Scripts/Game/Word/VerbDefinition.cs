using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct VerbState
{
    public WordObject Actor;
    public WordObject Target;
    public bool IsDone;
}

public class VerbDefinition : WordDefinition
{
    public override EWordType WordType => EWordType.Verb;

    public virtual void StartAction(ref VerbState state) { }
    public virtual void UpdateAction(ref VerbState state) { }
    public virtual void StopAction(ref VerbState state) { }
}
