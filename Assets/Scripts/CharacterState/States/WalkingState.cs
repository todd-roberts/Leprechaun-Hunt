using UnityEngine;

public class WalkingState : CharacterState
{
    private Vector3 _targetPosition;
    private float _elapsedTime = 0f;

    public WalkingState()
    {
        _targetPosition = Character.transform.position + Character.transform.forward * Character.GetWalkDistance();
    }

    public override void Enter()
    {
        Character.GetAnimator().Play("Walk");
    }

    public override void Update()
    {
        _elapsedTime += Time.deltaTime;

        Character.transform.position = Vector3.Lerp(
            Character.transform.position,
            _targetPosition,
            _elapsedTime / Character.GetWalkDuration()
        );

        if (_elapsedTime >= Character.GetWalkDuration())
        {
            Character.transform.position = _targetPosition;
            StateMachine.SetState(new IdleState());
        }
    }
}
