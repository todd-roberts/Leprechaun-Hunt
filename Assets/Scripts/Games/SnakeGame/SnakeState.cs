public abstract class SnakeState
{
    protected Snake _snake;
    protected SnakeStateMachine _stateMachine;

    public void Initialize(Snake snake, SnakeStateMachine stateMachine)
    {
        _snake = snake;
        _stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void OnPointerDown() { }
}
