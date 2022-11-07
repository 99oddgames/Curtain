using UnityEngine;

public class AudienceMember : MonoBehaviour
{
    [SerializeField]
    Animator _animator;

    [SerializeField]
    private float _calmDownTime = 2;

    private float _excitement = 0;

    void Start()
    {
        EventBetter.Listen<AudienceMember, NounEvents.Attack>(this, msg => OnVerb());
    }

    void Update()
    {
        if (_excitement > 0)
        {
            _excitement -= Time.deltaTime / _calmDownTime;
            _animator.SetFloat("Blend", _excitement);
        }
    }

    [EditorButton]
    private void OnVerb()
    {
        _excitement = Random.Range(0f, 1f);
    }
}
