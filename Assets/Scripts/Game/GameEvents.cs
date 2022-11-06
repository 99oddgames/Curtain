using UnityEngine;

//For use with the Pub-Sub pattern. See https://github.com/gwiazdorrr/EventBetter for usage examples

/// <summary>
/// Contains general purpose game events
/// </summary>
namespace GameEvents
{
    public struct PlaySound
    {
        public AudioClip Clip;
    }
}

/// <summary>
/// Contains gamemplay events targeting nouns
/// </summary>
namespace NounEvents
{
    public struct Attack
    {
        public enum EAttackType
        {
            Punch,
            Slash
        }

        public GameObject Target;

        public Vector3 Direction;
        public float Impulse;
        public EAttackType Type;
    }
}
