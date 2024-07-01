using UnityEngine;
using System;
using System.Collections.Generic;

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
    private GameState _gameState = GameState.Initial;

    public static event Action<GameState> OnGameStateChanged;

    private Dictionary<string, Action> _dialogueCallbacks;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _dialogueCallbacks = new Dictionary<string, Action>();
        }
        else
        {
            Destroy(gameObject);
        }
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

    public static void RegisterDialogueCallback(string key, Action callback)
    {
        _instance._dialogueCallbacks[key] = callback;
    }

    public static void TriggerDialogueCallback(string key)
    {
        if (_instance._dialogueCallbacks.TryGetValue(key, out Action callback))
        {
            callback?.Invoke();
        }
    }
}
