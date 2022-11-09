using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomClipOnEnable : MonoBehaviour
{
    public AudioClip[] Clips;

    private AudioSource source;

    public float Delay;
    private Duration delay;

    private void Awake()
    {
        source = GetComponentInChildren<AudioSource>();
    }

    private void OnEnable()
    {
        if (Clips.Length == 0)
            return;

        if (Delay > 0)
        {
            delay.Next(Delay);
            return;
        }

        int randomIndex = Random.Range(0, Clips.Length);
        source.clip = Clips[randomIndex];
        source.Play();
    }

    private void Update()
    {
        if(delay.Once)
        {
            if (Clips.Length == 0)
                return;

            int randomIndex = Random.Range(0, Clips.Length);
            source.clip = Clips[randomIndex];
            source.Play();
        }
    }
}
