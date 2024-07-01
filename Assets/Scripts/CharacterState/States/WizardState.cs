using UnityEngine;

public class WizardState : LeprechaunState
{
    private Vector3[] relativeTeleportPositions;
    private int lastPositionIndex = -1;
    private float teleportTimer;

    public override void Enter()
    {
        _leprechaun.transform.parent = null;
        InitializeTeleportPositions();
        Teleport();
    }

    private void InitializeTeleportPositions()
    {
        relativeTeleportPositions = new Vector3[]
        {
            new Vector3(-1, 0, 0), // LEFT
            new Vector3(1, 0, 0),  // RIGHT
            new Vector3(0, 0, -1)  // BEHIND
        };
    }

    private void Teleport()
    {
        int newPositionIndex;
        do
        {
            newPositionIndex = Random.Range(0, relativeTeleportPositions.Length);
        } while (newPositionIndex == lastPositionIndex);

        lastPositionIndex = newPositionIndex;
        Vector3 relativePosition = relativeTeleportPositions[newPositionIndex];

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0; // Keep the forward direction horizontal

        Vector3 right = Camera.main.transform.right;
        right.y = 0; // Keep the right direction horizontal

        Vector3 newPosition = Camera.main.transform.position +
                              (right * relativePosition.x + cameraForward * relativePosition.z) * _leprechaun.teleportDistance;
        newPosition.y = Camera.main.transform.position.y;

        _leprechaun.transform.position = newPosition;
        _leprechaun.PlayTeleportSound();

        // Reset teleport timer
        teleportTimer = 0f;
    }

    public override void Update()
    {
        teleportTimer += Time.deltaTime;

        if (teleportTimer >= _leprechaun.teleportDelay)
        {
            Teleport();
        }
    }

    public override void OnPointerDown()
    {
        Debug.Log("OnPointerDown");
        if (IsPlayerClose()) // Or any other touch input detection
        {
            _leprechaun.PlayAnimation("Touched");
            _leprechaun.PlayTouchedSound();
            _leprechaun.touchCounter++;
            if (_leprechaun.touchCounter >= _leprechaun.requiredTouches)
            {
                _stateMachine.SetState(new CaughtState());
            }
            else
            {
                Teleport(); // Teleport immediately after being touched
            }
        }
    }

    private bool IsPlayerClose()
    {
        float distance = Vector3.Distance(_leprechaun.transform.position, Camera.main.transform.position);
        return distance <= _leprechaun.touchDistance;
    }
}
