using System.Collections.Generic;
using UnityEngine;

public enum TeleportationPosition
{
    LEFT,
    RIGHT,
    BEHIND
}

public class TeleportingState : LeprechaunState
{
    private float scaleDownDuration = 0.2f;
    private float scaleDownTimer = 0f;
    private Dictionary<TeleportationPosition, Vector3> _teleportPositions = new Dictionary<
        TeleportationPosition,
        Vector3
    >
    {
        { TeleportationPosition.LEFT, new Vector3(-1, 0, 0) },
        { TeleportationPosition.RIGHT, new Vector3(1, 0, 0) },
        { TeleportationPosition.BEHIND, new Vector3(0, 0, -1) }
    };

    private Vector3 originalScale;
   
    private bool _firstTeleportation = true;

    private TeleportationPosition _teleportationPosition = TeleportationPosition.BEHIND;

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
        if (_firstTeleportation)
        {
            _firstTeleportation = false;
        }
        else
        {
            if (_teleportationPosition == TeleportationPosition.BEHIND)
            {
                _teleportationPosition =
                    Random.Range(0f, 1) > 0.5f
                        ? TeleportationPosition.LEFT
                        : TeleportationPosition.RIGHT;
            }
            else if (_teleportationPosition == TeleportationPosition.LEFT)
            {
                _teleportationPosition = Random.Range(0f, 1) > 0.5f
                    ? TeleportationPosition.RIGHT
                    : TeleportationPosition.BEHIND;
            }
            else
            {
                _teleportationPosition =
                    Random.Range(0f, 1) > 0.5f
                        ? TeleportationPosition.LEFT
                        : TeleportationPosition.BEHIND;
            }
        }

        return GetTeleportPosition();
    }

    private Vector3 GetTeleportPosition()
    {
        return _teleportPositions[_teleportationPosition];
    }
}
