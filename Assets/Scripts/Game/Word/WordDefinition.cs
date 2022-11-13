using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordDefinition : ScriptableObject
{
    [HideInInspector]
    public WordObject Owner; //populated at runtime for addressing

    public string Text;
    public Color BackgroundColor;

    public enum EWordType
    {
        Unspecified,
        Noun, 
        Verb,
        Adjective
    }

    public virtual EWordType WordType => EWordType.Unspecified;

    public virtual void OnWordPlaced() { }
    public virtual void OnWordPickedUp() { }
}
