using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterRig : MonoBehaviour
{
    [SerializeField]
    private GameObject _door;

    [SerializeField]
    private GameObject _doorKnob;

    private bool _doorIsOpen = false;
    private Coroutine _doorCoroutine;

    public static float maxDistance = 1f;
    public static float doorOpenDuration = 1f; // Duration to open the door

    protected virtual void Awake()
    {
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
        if (!_doorIsOpen)
        {
            if (_doorCoroutine != null)
            {
                StopCoroutine(_doorCoroutine);
            }
            _doorCoroutine = StartCoroutine(OpenDoorGradually());
            _doorIsOpen = true;
        }
    }

    private void Update()
    {
        CheckDespawnConditions();
    }

    public bool IsCloseToCamera()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        return distance <= maxDistance;
    }

    private void CheckDespawnConditions()
    {
        if (!IsCloseToCamera())
        {
            Despawn();
            return;
        }
    }

    private void Despawn()
    {
        _doorIsOpen = false;
        _door.transform.localRotation = Quaternion.identity;
        gameObject.SetActive(false);
    }

    private IEnumerator OpenDoorGradually()
    {
        Quaternion initialRotation = _door.transform.localRotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, 90, 0);

        float elapsedTime = 0f;

        while (elapsedTime < doorOpenDuration)
        {
            Quaternion slerped = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / doorOpenDuration);
            _door.transform.localRotation = slerped;
            _doorKnob.transform.localRotation = slerped;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _door.transform.localRotation = targetRotation;
        _doorKnob.transform.localRotation = targetRotation;
    }
}
