using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordDefinition : ScriptableObject
{
    public enum EWordType
    {
        Noun, 
        Verb,
        Adjective
    }

    public virtual void OnWordPlaced() { }
    public virtual void OnWordPickedUp() { }
}
