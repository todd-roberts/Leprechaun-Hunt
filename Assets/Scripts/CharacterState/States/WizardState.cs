using UnityEngine;

public class WizardState : LeprechaunState
{
    private float teleportTimer;

    public override void Enter()
    {
        _leprechaun.PlayAnimation("Unimpressed");
        _leprechaun.transform.parent = null;
        teleportTimer = 0f;
    }

    public override void Update()
    {
        teleportTimer += Time.deltaTime;
        if (teleportTimer >= _leprechaun.teleportDelay)
        {
            _stateMachine.SetState(new TeleportingState());
        }

        FaceCamera();
        _leprechaun.BobUpAndDown();
    }

    public override void OnPointerDown()
    {

        if (_leprechaun.IsPlayerClose())
        {
            _stateMachine.SetState(new TouchedState());
        }
    }
}
