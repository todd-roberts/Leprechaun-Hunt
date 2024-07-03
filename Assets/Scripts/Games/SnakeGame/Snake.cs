using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Snake : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audio;

    public int health = 3; // Snake's health

    [SerializeField]
    private AudioClip hitSound; // Sound played when snake is hit

    [SerializeField]
    private AudioClip deathSound; // Sound played when snake dies

    public float moveSpeed = 1f; // Speed at which the snake moves

    private Vector3 moveDirection; // Current move direction of the snake
    private Vector3 planeBoundsMin;
    private Vector3 planeBoundsMax;

    private SnakeStateMachine _stateMachine;

    public event Action<Snake> OnSnakeKilled;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();

        _stateMachine = new SnakeStateMachine(this);
        _stateMachine.SetState(new MovingState());

        SetupOnClickHandler();
    }

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

    private void Update()
    {
        _stateMachine.Update();
    }

    public void OnPointerDown()
    {
        _stateMachine.OnPointerDown();
    }

    public void Move()
    {
        transform.localPosition += moveDirection * moveSpeed * Time.deltaTime;
        CheckBounds();
    }

    private void RotateTowardsMoveDirection()
    {
        if (moveDirection != Vector3.zero)
        {
            transform.localRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        }
    }

    private void CheckBounds()
    {
        Vector3 localPosition = transform.localPosition;

        if (localPosition.x < planeBoundsMin.x || localPosition.x > planeBoundsMax.x)
        {
            moveDirection = Vector3.Reflect(moveDirection, Vector3.right);
            RotateTowardsMoveDirection();
        }

        if (localPosition.z < planeBoundsMin.z || localPosition.z > planeBoundsMax.z)
        {
            moveDirection = Vector3.Reflect(moveDirection, Vector3.forward);
            RotateTowardsMoveDirection();
        }

        // Snap the snake back onto the plane if it goes out of bounds slightly
        localPosition.x = Mathf.Clamp(localPosition.x, planeBoundsMin.x, planeBoundsMax.x);
        localPosition.z = Mathf.Clamp(localPosition.z, planeBoundsMin.z, planeBoundsMax.z);
        localPosition.y = 0;
        transform.localPosition = localPosition;
    }

    public void SetPlaneBounds(Transform plane)
    {
        MeshFilter meshFilter = plane.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            Vector3 planeSize = meshFilter.sharedMesh.bounds.size;
            planeBoundsMin = -planeSize / 2;
            planeBoundsMax = planeSize / 2;
        }
        transform.SetParent(plane); // Ensure snake is a child of the plane
    }

    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
        RotateTowardsMoveDirection(); // Immediately face the new direction
    }

    public void PlaySound(AudioClip clip)
    {
        _audio.PlayOneShot(clip);
    }

    public void PlayHitSound()
    {
        PlaySound(hitSound);
    }

    public void PlayDeathSound()
    {
        PlaySound(deathSound);
    }

    public void PlayAnimation(string animationName)
    {
        _animator.Play(animationName);
    }

    public void NotifySnakeKilled()
    {
        OnSnakeKilled?.Invoke(this);
    }
}
