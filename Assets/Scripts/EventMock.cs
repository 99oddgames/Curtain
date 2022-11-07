using UnityEngine;

[System.Serializable]
public class EventMock : MonoBehaviour
{
    [EditorButton]
    private void PlaySoundEvent(GameEvents.PlaySound sound)
    {
        EventBetter.Raise<GameEvents.PlaySound>(sound);
    }

    [EditorButton]
    private void PlayAttackEvent(NounEvents.Attack attack)
    {
        EventBetter.Raise<NounEvents.Attack>(attack);
    }
}
