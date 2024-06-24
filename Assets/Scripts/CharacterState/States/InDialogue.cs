public class InDialogueState : CharacterState
{
    public override void Enter()
    {
        DialogueManager.StartDialogue(_character);
    }

    public override void Update()
    {
        FaceCamera();
    }
}
