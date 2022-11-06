using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackScene : MonoBehaviour
{
    public NounObject[] GetNounObjects()
    {
        return GetComponentsInChildren<NounObject>();
    }
}
