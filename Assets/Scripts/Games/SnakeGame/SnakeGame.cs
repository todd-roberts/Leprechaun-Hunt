using UnityEngine;

public class SnakeGame : MonoBehaviour
{
    [SerializeField]
    private GameObject snakePrefab;

    [SerializeField]
    private int snakesPerPlane = 5; // Configurable number of snakes per plane

    [SerializeField]
    private int requiredSnakeKills = 20; // Configurable required snake kills

    [SerializeField]
    private Transform[] wallPlanes; // Assign the wall planes in the inspector

    private int snakeKills = 0;
    private bool started = false;
    private bool stateChangeHandlerSet = false;

    private void Awake()
    {
        SetGameStateHandler();
    }

    private void Start()
    {
        StartGame();
    }

    private void SetGameStateHandler()
    {
        if (stateChangeHandlerSet) return;

        GameManager.OnGameStateChanged += HandleGameStateChanged;
        stateChangeHandlerSet = true;
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.SnakeGameStarted)
        {
            StartGame();
        }
        else if (state == GameState.SnakeGameWon)
        {
            EndGame();
        }
    }

    private void StartGame()
    {
        if (!started)
        {
            started = true;
            SpawnSnakes();
        }
    }

    private void EndGame()
    {
        foreach (Transform plane in wallPlanes)
        {
            Destroy(plane.gameObject);
        }
    }

    private void SpawnSnakes()
    {
        foreach (Transform plane in wallPlanes)
        {
            for (int i = 0; i < snakesPerPlane; i++)
            {
                SpawnSnake(plane);
            }
        }
    }

    public void SpawnSnake(Transform plane)
    {
        GameObject snake = Instantiate(snakePrefab, GetRandomPositionOnPlane(plane), plane.rotation, plane);
        Snake snakeScript = snake.GetComponent<Snake>();
        snakeScript.SetMoveDirection(GetRandomDirection());
        snakeScript.SetPlaneBounds(plane);
        snakeScript.OnSnakeKilled += HandleSnakeKilled;
    }

    private void HandleSnakeKilled(Snake snake)
    {
        snakeKills++;
        if (snakeKills >= requiredSnakeKills)
        {
            GameManager.SetGameState(GameState.SnakeGameWon);
        }
        else
        {
            Transform plane = snake.transform.parent;
            SpawnSnake(plane);
        }
    }

    private Vector3 GetRandomPositionOnPlane(Transform plane)
    {
        MeshFilter meshFilter = plane.GetComponent<MeshFilter>();
        Vector3 planeSize = meshFilter.sharedMesh.bounds.size;
        float randomX = Random.Range(-planeSize.x / 2, planeSize.x / 2);
        float randomY = Random.Range(-planeSize.y / 2, planeSize.y / 2);
        return plane.position + plane.right * randomX + plane.up * randomY;
    }

    private Vector3 GetRandomDirection()
    {
        float angle = Random.Range(0f, 360f);
        return Vector3.right * Mathf.Cos(angle) + Vector3.forward * Mathf.Sin(angle);
    }
}
