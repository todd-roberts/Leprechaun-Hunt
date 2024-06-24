using UnityEngine;

public class WalkingState : CharacterState
{
    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private float _elapsedTime;

    public override void Enter()
    {
        _elapsedTime = 0;
        _initialPosition = _character.transform.position;
        _targetPosition = _character.transform.position + _character.transform.forward * _character.GetWalkDistance();
        _character.PlayAnimation("Walk");
    }

    public override void Update()
    {
        _elapsedTime += Time.deltaTime;

        _character.transform.position = Vector3.Lerp(
            _initialPosition,
            _targetPosition,
            _elapsedTime / _character.GetWalkDuration()
        );

        if (_elapsedTime >= _character.GetWalkDuration())
        {
            _character.transform.position = _targetPosition;
            _stateMachine.SetState(new IdleState());
        }
    }
}
