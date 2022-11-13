using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//just a sample, everything here will be changed
public class WordObject : MonoBehaviour, IRewindableObject
{
    public WordDefinition WordDefinition;

    public Rigidbody Body;

    RecordedTransformState transformState;

    public void RecordState()
    {
        transformState.Record(transform);
    }

    public void Rewind()
    {
        transformState.Restore(transform);
        Body.velocity = Vector3.zero;
        Body.angularVelocity = Vector3.zero;
    }

    private void Start()
    {
        EventBetter.Listen<WordObject, NounEvents.Attack>(this, msg =>
        {
            if (msg.Target != gameObject)
                return;

            Body.AddForce(msg.Direction * msg.Impulse, ForceMode.Impulse);
            Debug.Log($"I've been attacked by ({msg.Type}) in direction ({msg.Direction}), with strength {msg.Impulse} ");
        });
    }
}
