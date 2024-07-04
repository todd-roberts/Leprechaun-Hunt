using System.Collections.Generic;
using UnityEngine;

public enum TeleportationPosition
{
    LEFT,
    RIGHT
}

public class TeleportingState : LeprechaunState
{
    private float scaleDownDuration = 0.2f;
    private float scaleDownTimer = 0f;
    private Dictionary<TeleportationPosition, Vector3> _teleportPositions = new Dictionary<TeleportationPosition, Vector3>
    {
        { TeleportationPosition.LEFT, new Vector3(-1, 0, 0) },
        { TeleportationPosition.RIGHT, new Vector3(1, 0, 0) }
    };

    private Vector3 originalScale;

    public override void Enter()
    {
        scaleDownTimer = 0f;
        originalScale = _leprechaun.transform.localScale;
    }

    public override void Update()
    {
        scaleDownTimer += Time.deltaTime;
        float scaleFraction = scaleDownTimer / scaleDownDuration;
        _leprechaun.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, scaleFraction);

        if (scaleFraction >= 1f)
        {
            _leprechaun.transform.localScale = Vector3.zero;
            _leprechaun.PlayTeleportSound();

            _leprechaun.MoveToRelativePosition(GetNewPosition());

            _leprechaun.transform.localScale = originalScale;
            _stateMachine.SetState(new WizardState());
        }
    }

    private Vector3 GetNewPosition()
    {
        // Randomly select LEFT or RIGHT
        TeleportationPosition newPosition = Random.Range(0, 2) == 0 ? TeleportationPosition.LEFT : TeleportationPosition.RIGHT;
        return _teleportPositions[newPosition];
    }
}
