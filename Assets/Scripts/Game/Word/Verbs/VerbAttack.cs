using UnityEngine;

[CreateAssetMenu(fileName = "VerbAttack", menuName = "Verbs/Attack")]
public class VerbAttack : VerbDefinition
{
    [Header("Gameplay")]
    public NounEvents.Attack.EAttackType Type;
    public float Impulse;
    public float StoppingDistance = 1f;
    public Duration Duration = new Duration(1f, 1f);

    private MovementAnim movementAnim;
    private Vector3 movementDirection;

    public override void StartAction(ref VerbState state)
    {
        if(state.Target == null)
        {
            Debug.Log("Attack with no target not implemented");
            state.IsDone = true;
            return;
        }

        Duration.Next();

        movementDirection = (state.Target.transform.position - state.Actor.transform.position).normalized;
        movementAnim = new MovementAnim()
        {
            From = state.Actor.transform.position,
            To = state.Target.transform.position - movementDirection * StoppingDistance
        }; 
    }

    public override void UpdateAction(ref VerbState state)
    {
        state.Actor.transform.position = movementAnim.GetPoint(Duration.ElapsedQuad);
        state.IsDone = Duration.IsUp;
    }

    public override void StopAction(ref VerbState state)
    {
        EventBetter.Raise(new NounEvents.Attack()
        {
            Type = Type,
            Target = state.Target.gameObject,
            Direction = movementDirection,
            Impulse = Impulse,
        });
    }
}
