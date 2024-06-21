using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public abstract class Character : MonoBehaviour
{
    protected Animator _animator;

    [SerializeField]
    private float walkDistance = 0.2159f; // Distance to walk
    [SerializeField]
    private float walkDuration = 2f; // Duration of the walk

    private Vector3 _initialPosition;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _initialPosition = transform.localPosition;
        SetupOnClickHandler();
    }

    private void SetupOnClickHandler()
    {
        if (!TryGetComponent<EventTrigger>(out var eventTrigger))
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry pointerDownEntry = new()
        {
            eventID = EventTriggerType.PointerDown
        };

        pointerDownEntry.callback.AddListener((data) => OnPointerDown());

        eventTrigger.triggers.Add(pointerDownEntry);
    }

    public void Walk()
    {
        StartCoroutine(WalkCoroutine());
    }

    private IEnumerator WalkCoroutine()
    {
        _animator.Play("Walk");
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + transform.forward * walkDistance;

        float elapsedTime = 0f;

        while (elapsedTime < walkDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / walkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        _animator.Play("Idle");
    }

    public void ResetPosition()
    {
        transform.localPosition = _initialPosition;
    }

    public void OnPointerDown()
    {
        GameState currentState = GameManager.GetGameState();
        ProcessGameState(currentState);
    }

    protected abstract void ProcessGameState(GameState gameState);
}
