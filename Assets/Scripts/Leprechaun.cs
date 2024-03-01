using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum LeprechaunAnimation
{
    Idle,
    Wave
}

public class Leprechaun : MonoBehaviour, IPointerDownHandler
{
    private Animator _animator;

    [SerializeField]
    private AudioClip[] _audioClips;

    private readonly Dictionary<LeprechaunAnimation, string> _animationNameMap = new()
    {
        { LeprechaunAnimation.Idle, "Lep_looking_around" },
        { LeprechaunAnimation.Wave, "Lep_waving_B" }
    };

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayAnimation(LeprechaunAnimation.Wave);
    }

    public void PlayAnimation(LeprechaunAnimation animation)
    {
        _animator.Play(_animationNameMap[animation]);
    }
}
