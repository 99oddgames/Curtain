using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRewindableObject
{
    public void RecordState();
    public void Rewind();
}

public struct RecordedTransformState
{
    public Vector3 Position;
    public Quaternion Rotation;

    public void Record(Transform transform)
    {
        Position = transform.position;
        Rotation = transform.rotation;
    }

    public void Restore(Transform transform)
    {
        transform.position = Position;
        transform.rotation = Rotation;
    }
}