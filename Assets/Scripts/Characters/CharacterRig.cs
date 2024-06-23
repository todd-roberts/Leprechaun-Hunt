using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class CharacterRig : MonoBehaviour
{
    private CharacterSpawner _characterSpawner;

    private AudioSource _audio;

    [SerializeField]
    private GameObject _door;

    [SerializeField]
    private AudioClip _doorOpenSound;

    [SerializeField]
    private AudioClip _knockSound;

    [SerializeField]
    private Character _character;

    private bool _doorIsOpen = false;
    private Coroutine _doorCoroutine;
    private Coroutine _knockCoroutine;

    private int _knockCount = 0;
    private float _knockThreshold = 0.5f; // Time within which the second knock should happen

    public static float maxDistance = 1f;
    public static float doorOpenDuration = 1f; // Duration to open the door

    protected virtual void Awake()
    {
        _characterSpawner = FindObjectOfType<CharacterSpawner>();
        _audio = GetComponent<AudioSource>();
        _character.gameObject.SetActive(false);
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
        if (_doorIsOpen) return;

        _audio.PlayOneShot(_knockSound);
        _knockCount++;

        if (_knockCoroutine != null)
        {
            StopCoroutine(_knockCoroutine);
        }

        _knockCoroutine = StartCoroutine(KnockResetCoroutine());

        if (_knockCount == 2)
        {
            _knockCount = 0;
            if (_doorCoroutine != null)
            {
                StopCoroutine(_doorCoroutine);
            }
            _audio.PlayOneShot(_doorOpenSound);
            _doorCoroutine = StartCoroutine(OpenDoorGradually());
            _doorIsOpen = true;
        }
    }

    private IEnumerator KnockResetCoroutine()
    {
        yield return new WaitForSeconds(_knockThreshold);
        _knockCount = 0;
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
        }
    }

    private void Despawn()
    {
        DialogueManager.Despawn();
        _characterSpawner.Poof(transform.position);
        _doorIsOpen = false;
        _door.transform.localRotation = Quaternion.identity;
        _character.ResetPosition();
        _character.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private IEnumerator OpenDoorGradually()
    {
        Quaternion initialRotation = _door.transform.localRotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, 150, 0);

        float elapsedTime = 0f;

        while (elapsedTime < doorOpenDuration)
        {
            Quaternion slerped = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / doorOpenDuration);
            _door.transform.localRotation = slerped;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _door.transform.localRotation = targetRotation;

        _character.gameObject.SetActive(true);
        _character.Walk();
        
    }
}
