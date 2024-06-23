using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Character : MonoBehaviour
{
    protected CharacterStateMachine _stateMachine;
    protected Animator _animator;

    [SerializeField]
    private float walkDistance = 0.2159f; // Distance to walk
    [SerializeField]
    private float walkDuration = 2f; // Duration of the walk

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _stateMachine = new CharacterStateMachine(this);

        ResetPosition();
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

    void Update() {
        _stateMachine.Update();
    }

    public void OnPointerDown()
    {
        _stateMachine.OnPointerDown();
    }

    public void ResetPosition()
    {
        transform.position = transform.parent.position - transform.forward * walkDistance;
    }

    public void Walk() {
        _stateMachine.SetState(new WalkingState());
    }

    public float GetWalkDistance() => walkDistance;

    public float GetWalkDuration() => walkDuration; 

    public Animator GetAnimator() => _animator;
}

