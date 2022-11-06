using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//just a sample, everything here will be changed
public class NounObject : MonoBehaviour
{
    public Rigidbody Body;

    private void Start()
    {
        EventBetter.Listen<NounObject, NounEvents.Attack>(this, msg =>
        {
            if (msg.Target != gameObject)
                return;

            Body.AddForce(msg.Direction * msg.Impulse, ForceMode.Impulse);
            Debug.Log($"I've been attacked by ({msg.Type}) in direction ({msg.Direction}), with strength {msg.Impulse} ");
        });
    }
}
