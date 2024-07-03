public class SnakeStateMachine
{
    private Snake _snake;
    private SnakeState _currentState;

    public SnakeStateMachine(Snake snake)
    {
        _snake = snake;
    }

    public void SetState(SnakeState state)
    {
        _currentState?.Exit();

        _currentState = state;
        _currentState.Initialize(_snake, this);
        _currentState.Enter();
    }

    public void Update()
    {
        _currentState?.Update();
    }

    public void OnPointerDown()
    {
        _currentState?.OnPointerDown();
    }
}
