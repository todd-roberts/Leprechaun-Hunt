public class MovingState : SnakeState
{
    public override void Enter()
    {
        _snake.PlayAnimation("Run");
    }

    public override void Update()
    {
        _snake.Move();
    }

    public override void OnPointerDown()
    {
        _stateMachine.SetState(new DamagedState());
    }
}
