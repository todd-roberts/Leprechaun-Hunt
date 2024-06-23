using UnityEngine;

public class IdleState : CharacterState
{
    public override void Enter()
    {
        Character.GetAnimator().Play("Idle");
    }

    public override void Update()
    {
        FaceCamera();
    }

    private void FaceCamera()
    {
        Vector3 direction = Camera.main.transform.position - Character.transform.position;
        direction.y = 0; // Keep only the horizontal direction
        Quaternion rotation = Quaternion.LookRotation(direction);
        Character.transform.rotation = Quaternion.Slerp(Character.transform.rotation, rotation, Time.deltaTime * 2);
    }

    public override void OnPointerDown()
    {
        // Handle dialogue per GameState / Dialogue State
    }
}
