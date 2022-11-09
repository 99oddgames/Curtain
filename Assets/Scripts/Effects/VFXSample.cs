using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSample : MonoBehaviour
{
    public VFX SampleEffect;
    public Duration Timer = new Duration(1f);

    public void Update()
    {
        if (!Timer.IsUp)
            return;

        Timer.Next();
        SampleEffect.SafeSpawn(transform.position, Quaternion.LookRotation(transform.forward));
    }
}
