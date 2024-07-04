public class DyingState : SnakeState
{
    public override void Enter()
    {
        _snake.PlayDeathSound();
        _snake.PlayAnimation("Death");
        _snake.NotifySnakeKilled();
    }
}
