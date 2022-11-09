using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct VFXScreenshake
{
    public bool Play;

    [Range(0, 1f)]
    public float Trauma;
}

public class VFX : PoolableObject
{
    public Duration Delay;

    [Header("Screenshake")]

    public VFXScreenshake Screenshake;

    private AudioSource[] audioSources = null;
    private ParticleSystem[] particleSystems = null;

    private void Awake()
    {
        audioSources = GetComponentsInChildren<AudioSource>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        if (!Screenshake.Play)
            return;

        if (Delay.Min > 0f)
        {
            Delay.Next();
        }
        else
        {
            TriggerFrontloadedEffects();
        }
    }

    private void TriggerFrontloadedEffects()
    {
        if(Screenshake.Play) { } //TODO screenshake
    }

    public void Update()
    {
        if(Delay.Once)
        {
            TriggerFrontloadedEffects();
        }

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].isPlaying)
                return;
        }

        for (int i = 0; i < particleSystems.Length; i++)
        {
            if (particleSystems[i].isPlaying)
                return;
        }

        Despawn();
    }
}
