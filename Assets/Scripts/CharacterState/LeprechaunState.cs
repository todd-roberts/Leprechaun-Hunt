public abstract class LeprechaunState : CharacterState
{
    protected Leprechaun _leprechaun;

    public override void Initialize(Character character, CharacterStateMachine stateMachine)
    {
        base.Initialize(character, stateMachine);
        _leprechaun = character as Leprechaun;
    }
}
