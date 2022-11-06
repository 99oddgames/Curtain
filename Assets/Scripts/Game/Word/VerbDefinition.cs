using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct VerbState
{
    public GameObject Actor;
    public GameObject Target;
    public bool IsDone;
}

public class VerbDefinition : WordDefinition
{
    public virtual void StartAction(ref VerbState state) { }
    public virtual void UpdateAction(ref VerbState state) { }
    public virtual void StopAction(ref VerbState state) { }
}
