public abstract class CharacterState
{
    public Character Character { get; set; }
    public CharacterStateMachine StateMachine { get; set; }

    public virtual void Enter() {}

    public virtual void Exit() { }

    public virtual void Update() { }

    public virtual void OnPointerDown() { }
}
