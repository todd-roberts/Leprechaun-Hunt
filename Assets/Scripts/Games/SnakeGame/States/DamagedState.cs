using UnityEngine;

public class DamagedState : SnakeState
{
    private float _maxDamagedTime = 1f;
    private float _damagedTime;

    public override void Enter()
    {
        Damage();
    }

    public void Damage()
    {
        _snake.health--;

        if (_snake.health <= 0)
        {
            _stateMachine.SetState(new DyingState());
        }
        else
        {
            _snake.PlayHitSound();
            _damagedTime = 0f; // Reset the damaged time
            _snake.moveSpeed *= 0.5f;
        }
    }

    public override void Update()
    {
        _damagedTime += Time.deltaTime;

        if (_damagedTime >= _maxDamagedTime)
        {
            _stateMachine.SetState(new MovingState());
        }
    }
}
