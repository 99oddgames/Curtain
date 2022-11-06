using UnityEngine;

//better than dotween btw
public struct MovementAnim
{
    public Vector3 From;
    public Vector3 To;

    public Vector3 GetPoint(float t01)
    {
        return Vector3.Lerp(From, To, t01);
    }
}