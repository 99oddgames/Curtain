using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackScene : MonoBehaviour
{
    public WordObject[] GetNounObjects()
    {
        return GetComponentsInChildren<WordObject>();
    }
}
