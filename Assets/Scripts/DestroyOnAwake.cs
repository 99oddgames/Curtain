using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAwake : MonoBehaviour
{
    public GameObject[] ToDestroy;

    private void Awake()
    {
        for(int i = 0; i < ToDestroy.Length; i++)
        {
            Destroy(ToDestroy[i]);
        }
    }
}
