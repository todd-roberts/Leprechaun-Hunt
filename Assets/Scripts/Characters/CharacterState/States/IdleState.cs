using UnityEngine;

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

    private void FaceCamera()
    {
        Vector3 direction = Camera.main.transform.position - _character.transform.position;
        direction.y = 0; // Keep only the horizontal direction
        Quaternion rotation = Quaternion.LookRotation(direction);
        _character.transform.rotation = Quaternion.Slerp(_character.transform.rotation, rotation, Time.deltaTime * 2);
    }

    public override void OnPointerDown()
    {
        // Handle dialogue per GameState / Dialogue State
    }
}
