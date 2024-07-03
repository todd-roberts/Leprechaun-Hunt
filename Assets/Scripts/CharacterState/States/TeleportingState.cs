using UnityEngine;

public class TeleportingState : LeprechaunState
{
    private float scaleDownDuration = 0.2f;
    private float scaleDownTimer = 0f;
    private Vector3[] relativeTeleportPositions = new Vector3[]
    {
        new Vector3(-1, 0, 0), // LEFT
        new Vector3(1, 0, 0),  // RIGHT
        new Vector3(0, 0, -1)  // BEHIND
    };
    private int lastPositionIndex = -1;
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
            _leprechaun.MoveToRelativePosition(_leprechaun.GetRandomRelativePosition(relativeTeleportPositions, ref lastPositionIndex));
            _leprechaun.transform.localScale = originalScale;
            _stateMachine.SetState(new WizardState());
        }
    }
}
