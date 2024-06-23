public class IdleState : CharacterState
{
    public override void Enter()
    {
        _character.PlayAnimation("Idle");
    }

    public override void Update()
    {
        FaceCamera();
    }

    public override void OnPointerDown()
    {
        _stateMachine.SetState(new InDialogueState());
    }
}
