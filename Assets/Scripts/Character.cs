using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public abstract class Character : MonoBehaviour
{
    protected Animator _animator;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
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


    public void OnPointerDown()
    {
        GameState currentState = GameManager.GetGameState();
        ProcessGameState(currentState);
    }

    protected abstract void ProcessGameState(GameState gameState);
}
