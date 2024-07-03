using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public abstract class Character : MonoBehaviour
{
    protected CharacterStateMachine _stateMachine;
    protected Animator _animator;

    [SerializeField]
    private float walkDistance = 0.2159f; // Distance to walk

    [SerializeField]
    private float walkDuration = 2f; // Duration of the walk

    [SerializeField]
    private List<GameStateDialogue> gameStateDialogues;

    private Dictionary<GameState, DialogueSet> _dialogues;

    private bool _detached = false;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _stateMachine = new CharacterStateMachine(this);

        SetupOnClickHandler();
        InitializeDialogues();
        ResetPosition();

        OnAwake();
    }

    protected virtual void OnAwake() { }

    private void SetupOnClickHandler()
    {
        if (!TryGetComponent<EventTrigger>(out var eventTrigger))
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry pointerDownEntry = new() { eventID = EventTriggerType.PointerDown };

        pointerDownEntry.callback.AddListener((data) => OnPointerDown());

        eventTrigger.triggers.Add(pointerDownEntry);
    }

    private void InitializeDialogues()
    {
        _dialogues = new Dictionary<GameState, DialogueSet>();

        foreach (var gameStateDialogue in gameStateDialogues)
        {
            _dialogues[gameStateDialogue.gameState] = new DialogueSet(
                new List<DialogueEntry>(gameStateDialogue.dialogues)
            );
        }
    }

    public void ResetPosition()
    {
        transform.localPosition = new Vector3(0, 0, -walkDistance);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    public void OnPointerDown()
    {
        _stateMachine.OnPointerDown();
    }

    public void Walk()
    {
        _stateMachine.SetState(new WalkingState());
    }

    public float GetWalkDistance() => walkDistance;

    public float GetWalkDuration() => walkDuration;

    public virtual void PlayAnimation(string animationName)
    {
        _animator.Play(animationName);

        OnPlayAnimation(animationName);
    }

    protected virtual void OnPlayAnimation(string animationName) { }

    public DialogueSet GetDialogueSet() => _dialogues[GameManager.GetGameState()];

    private CharacterRig GetCharacterRig() => transform.parent.parent.GetComponent<CharacterRig>();

    public void Idle() => _stateMachine.SetState(new IdleState());

    public bool IsDetached() => _detached;

    public void Detach() {
        _detached = true;

        CharacterRig characterRig = GetCharacterRig();
        
        transform.parent = null;
        
        characterRig.Despawn();
    }
}
