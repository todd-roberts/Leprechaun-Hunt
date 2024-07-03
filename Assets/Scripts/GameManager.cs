using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public enum GameState
{
    Initial,
    FindFourLeafClover,
    FourLeafCloverFound,
    FindRabbitsFoot,
    SnakeGameStarted,
    SnakeGameWon,
    RabbitsFootFound,
    FindHorseShoe,
    HorseshoeFound,
    CatchLeprechaun,
    LeprechaunCaught,
    RainbowVisionGranted
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField]
    private GameState _gameState;

    public static event Action<GameState> OnGameStateChanged;

    private Dictionary<string, Func<IEnumerator>> _dialogueCallbacks;

    public GameObject rainbowVisionPanel;

    private AudioSource _audio;

    [SerializeField]
    private AudioClip _successSound;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _dialogueCallbacks = new Dictionary<string, Func<IEnumerator>>();
            _audio = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start() {
        SetGameState(_gameState);
    }

    public static void SetGameState(GameState state)
    {
        if (_instance._gameState != state)
        {
            _instance._gameState = state;
            OnGameStateChanged?.Invoke(state);
        }
    }

    public static GameState GetGameState()
    {
        return _instance._gameState;
    }

    public static void RegisterDialogueCallback(string key, Func<IEnumerator> callback)
    {
        _instance._dialogueCallbacks[key] = callback;
    }

    public static IEnumerator TriggerDialogueCallback(string key)
    {
        if (_instance._dialogueCallbacks.TryGetValue(key, out Func<IEnumerator> callback))
        {
            if (callback != null)
            {
                yield return _instance.StartCoroutine(callback());
            }
        }
    }

    public static GameObject GetRainbowVisionPanel()
    {
        return _instance.rainbowVisionPanel;
    }

    public static void PlaySuccessSound()
    {
        _instance._audio.PlayOneShot(_instance._successSound);
    }
}
