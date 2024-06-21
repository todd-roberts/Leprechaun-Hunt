using UnityEngine;

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
        _instance._gameState = state;
    }

    public static GameState GetGameState()
    {
        return _instance._gameState;
    }
}
