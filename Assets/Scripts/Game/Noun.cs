using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noun : MonoBehaviour
{
    private void Start()
    {
        EventBetter.Listen<Noun, NounEvents.Attack>(this, msg =>
        {
            Debug.Log($"I've been attacked by ({msg.Type}) in direction ({msg.Direction}), with strength {msg.Strength} ");
        });

        EventBetter.Raise(new NounEvents.Attack()
        {
            Direction = Vector3.up,
            Strength = 100f,
            Target = gameObject,
            Type = NounEvents.Attack.EAttackType.Punch
        });
    }
}
