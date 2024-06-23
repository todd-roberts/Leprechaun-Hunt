public class CharacterStateMachine
{
    private Character _character;
    private CharacterState _currentState;

    public CharacterStateMachine(Character character)
    {
        _character = character;
    }

    public void SetState(CharacterState state)
    {
        _currentState?.Exit();
          
        state.StateMachine = this;
        state.Character = _character;
        
        _currentState = state;
        _currentState.Enter();
    }

    public void OnPointerDown() {
        _currentState?.OnPointerDown();
    }

    public void Update()
    {
        _currentState?.Update();
    }
}
