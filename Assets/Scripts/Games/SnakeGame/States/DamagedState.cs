using UnityEngine;

public class DamagedState : SnakeState
{
    private float _maxDamagedTime = 2f;
    private float _damagedTime;
    private Vector3 _originalScale;
    private Vector3 _damagedScale;

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

            _originalScale = _snake.transform.localScale;
            _damagedScale = new Vector3(_originalScale.x, _originalScale.y * 0.01f, _originalScale.z);
            _snake.transform.localScale = _damagedScale;
            _damagedTime = 0f; // Reset the damaged time
            _snake.moveSpeed *= 0.5f;
        }
    }

    public override void Update()
    {
        _damagedTime += Time.deltaTime;

        float t = _damagedTime / _maxDamagedTime;

        if (_damagedTime > _maxDamagedTime / 2)
        {
            _snake.transform.localScale = Vector3.Lerp(_damagedScale, _originalScale, (t - 0.5f) * 2);
        }
  
        if (_damagedTime >= _maxDamagedTime)
        {
            _stateMachine.SetState(new MovingState());
        }
    }
}
