using UnityEngine;

public class TouchedState : LeprechaunState
{
    private float animationDuration = 1f; // Duration of the touched animation and sound
    private float timer = 0f;

    public override void Enter()
    {
        timer = 0f;
        _leprechaun.touchCounter++;

        if (_leprechaun.touchCounter >= _leprechaun.requiredTouches)
        {
            _stateMachine.SetState(new CaughtState());
        } else {
             _leprechaun.PlayAnimation("Touched");
            _leprechaun.PlayTouchedSound();
        }
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer >= animationDuration)
        {
            if (_leprechaun.touchCounter >= _leprechaun.requiredTouches)
            {
                _stateMachine.SetState(new CaughtState());
            }
            else
            {
                _stateMachine.SetState(new TeleportingState());
            }
        }
    }
}
