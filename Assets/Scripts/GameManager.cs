using UnityEngine;
using System;

public enum GameState
{
    Initial,
    FindFourLeafClover,
    FourLeafCloverFound,
    FindRabbitsFoot,
    RabbitsFootFound,
    FindHorseShoe,
    HorseShoeFound,
    LeprechaunCaught,
    PotOfGoldFound,
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private GameState _gameState = GameState.Initial;

    public static event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
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
}
