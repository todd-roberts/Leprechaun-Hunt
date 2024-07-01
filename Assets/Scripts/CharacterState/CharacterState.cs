using UnityEngine;

public abstract class CharacterState
{

    protected Character _character;
    protected CharacterStateMachine _stateMachine;

    public virtual void Initialize(Character character, CharacterStateMachine stateMachine)
    {
        _character = character;
        _stateMachine = stateMachine;
    }

    public virtual void Enter() {}

    public virtual void Exit() { }

    public virtual void Update() { }

    public virtual void OnPointerDown() { }

    protected void FaceCamera()
    {
        Vector3 direction = Camera.main.transform.position - _character.transform.position;
        direction.y = 0; // Keep only the horizontal direction
        Quaternion rotation = Quaternion.LookRotation(direction);
        _character.transform.rotation = Quaternion.Slerp(_character.transform.rotation, rotation, Time.deltaTime * 2);
    }
}
