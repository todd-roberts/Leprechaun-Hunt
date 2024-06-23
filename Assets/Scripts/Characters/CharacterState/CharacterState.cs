public abstract class CharacterState
{

    protected Character _character;
    protected CharacterStateMachine _stateMachine;

    public void Initialize(Character character, CharacterStateMachine stateMachine)
    {
        _character = character;
        _stateMachine = stateMachine;
    }

    public virtual void Enter() {}

    public virtual void Exit() { }

    public virtual void Update() { }

    public virtual void OnPointerDown() { }
}
