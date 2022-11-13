using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultNoun", menuName = "Nouns/Default")]
public class NounDefinition : WordDefinition
{
    public override EWordType WordType => EWordType.Noun;
}
